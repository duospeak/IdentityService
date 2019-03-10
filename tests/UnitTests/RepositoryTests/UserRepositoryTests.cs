using Domain.AggregatesModel;
using DotNetCore.CAP;
using Infrastructure.EntityFrameworkCore;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Internal;
using Xunit;

namespace UnitTests.RepositoryTests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly IdentityServiceContext _context;
        private readonly ServiceProvider _container;
        public UserRepositoryTests()
        {
            _container = new ServiceCollection()
                .AddSingleton(Mock.Of<ICapPublisher>())
                .AddDbContext<IdentityServiceContext>(options =>
                {
                    options.UseInMemoryDatabase("xunit-user-repository-test-database");
                }).BuildServiceProvider(false);

            _context = _container.GetRequiredService<IdentityServiceContext>();
        }

        private UserRepository CreateRepository()
        {
            return new UserRepository(_context);
        }

        [Fact]
        public void Property_UnitOfWork_MustBe_IdentityServiceContext()
        {
            var repository = CreateRepository();

            Assert.Same(_context, repository.UnitOfWork);
        }

        [Fact]
        public async Task AllMethods_ArgsNull_Throws_ArgumentNullException()
        {
            var repository = CreateRepository();

            var argumentNullException = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetAsync(null));
            var argumentNullException1 = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null));


            Assert.NotNull(argumentNullException);
            Assert.NotNull(argumentNullException1);
        }

        [Fact]
        public async Task GetAsync_UnknownUserName_Return_Null()
        {
            var repository = CreateRepository();

            var result = await repository.GetAsync("unknown-user-name");

            Assert.Null(result);
        }
        [Fact]
        public async Task GetAsync_Ok()
        {
            // arrange
            var repository = CreateRepository();
            var user = Factory.CreateTestApplicationUser("added-user-name");
            await _context.ApplicationUser.AddAsync(user);
            await _context.SaveChangesAsync();

            // act
            var result = await repository.GetAsync("added-user-name");

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, user.Id);
            Assert.Equal(result.Password, user.Password);
            Assert.Equal(result.UserName, user.UserName);
        }

        [Fact]
        public async Task AddAsync_Ok()
        {
            // arrange
            var repository = CreateRepository();
            var user = Factory.CreateTestApplicationUser("added-user-name");

            // act
            var result = await repository.AddAsync(user);

            // assert
            var users = _context.ChangeTracker.Entries<ApplicationUser>();
            Assert.Equal(EntityState.Added, Assert.Single(users).State);

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, user.Id);
            Assert.Equal(result.Password, user.Password);
            Assert.Equal(result.UserName, user.UserName);
        }

        public void Dispose()
        {
            _container.Dispose();

            _context.Dispose();
        }
    }
}
