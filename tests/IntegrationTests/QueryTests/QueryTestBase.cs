using DotNetCore.CAP;
using Infrastructure.EntityFrameworkCore;
using IntegrationTests.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.QueryTests
{
    public abstract class QueryTestBase:IDisposable
    {
        public QueryTestBase()
        {
            ServiceContainer= new ServiceCollection()
                .AddDbContext<IdentityServiceContext>(options => {
                    options.UseNpgsql(TestConfig.ConnectionString);
                }).AddSingleton<ICapPublisher>(new TestCapPublisher())
                .BuildServiceProvider(false);

            IdentityDbContext = ServiceContainer.GetRequiredService<IdentityServiceContext>();

            IdentityDbContext.Database.EnsureCreated();
        }

        protected readonly ServiceProvider ServiceContainer;
        protected readonly IdentityServiceContext IdentityDbContext;

        public virtual void Dispose()
        {
            IdentityDbContext.Database.EnsureDeleted();
            ServiceContainer.Dispose();
        }
    }
}
