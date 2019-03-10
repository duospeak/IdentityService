using Domain.AggregatesModel;
using Domain.Events;
using DotNetCore.CAP;
using Infrastructure.EntityFrameworkCore;
using IntegrationTests.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class UnitOfWorkSaveEntitiesTests : IDisposable
    {
        private readonly ServiceProvider _container;
        private readonly IBootstrapper _bootstrapper;
        private readonly XunitTestDomainConsumer _consumerAssert;
        private readonly CancellationTokenSource _cts;
        public UnitOfWorkSaveEntitiesTests()
        {
            _consumerAssert = new XunitTestDomainConsumer();

            _container = new ServiceCollection()
                .AddSingleton<ICapSubscribe>(_consumerAssert)
                .AddCap(options =>
                {
                    options.UsePostgreSql(TestConfig.ConnectionString);

                    options.UseRabbitMQ(TestConfig.ConnectionFactory.HostName);
                }).Services.BuildServiceProvider(false);

            _bootstrapper = _container.GetServices<IHostedService>().FirstOrDefault(x => x is IBootstrapper) as IBootstrapper;

            _cts = new CancellationTokenSource();
        }

        private IdentityServiceContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<IdentityServiceContext>()
                .UseInMemoryDatabase("xunit-database");

            return new IdentityServiceContext(builder.Options, _container.GetRequiredService<ICapPublisher>());
        }


        [ExternalResourceFact(new[] { ExternalResource.Postgresql, ExternalResource.RabbitMQ })]
        public async Task SaveEntities_Ok()
        {
            // arrange
            bool assertEvent = false;
            var context = CreateContext();
            var applicationUser = new ApplicationUser("xunit-user-name", "password");
            applicationUser.ClearDomainEvents();
            applicationUser.AddDomainEvent(new XunitTestDomainEvent(applicationUser.Id, applicationUser.CreateTime));
            await context.ApplicationUser.AddAsync(applicationUser);

            if (_bootstrapper != null)
            {
                await ((IHostedService)_bootstrapper).StartAsync(_cts.Token);
                assertEvent = true;
            }

            // act
            await context.SaveEntitiesAsync();

            // assert
            if (assertEvent)
            {
                SpinWait.SpinUntil(() => _consumerAssert.AcceptEvent != null, 2000);

                Assert.NotNull(_consumerAssert.AcceptEvent);
            }
            // TODO: Query cap.publisher table , ensure event log was created
        }

        public void Dispose()
        {
            _cts.Cancel();
            _container.Dispose();
        }

        private class XunitTestDomainEvent : DomainEvent
        {
            public XunitTestDomainEvent(long id, DateTimeOffset createTime)
            {
                Id = id;
                CreateTime = createTime;
            }

            public long Id { get; set; }

            public DateTimeOffset CreateTime { get; set; }

        }

        private class XunitTestDomainConsumer : ICapSubscribe
        {
            public XunitTestDomainEvent AcceptEvent { get; private set; }

            [CapSubscribe(nameof(XunitTestDomainEvent))]
            public Task ProcessEventAsync(XunitTestDomainEvent @event)
            {
                if (AcceptEvent != null)
                {
                    AcceptEvent = @event;
                }

                return Task.CompletedTask;
            }
        }
    }
}
