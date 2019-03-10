using Application.Commands;
using Domain.AggregatesModel;
using Domain.Enumerations;
using Domain.Exceptions;
using IdentityServer4.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Internal;
using Xunit;

namespace UnitTests.CommandTests
{
    public class SignInCommandTests : IDisposable
    {
        private readonly MockRepository _repository;
        private readonly Mock<IUserRepository> _repositoryMock;
        public SignInCommandTests()
        {
            _repository = new MockRepository(MockBehavior.Strict);
            _repositoryMock = _repository.Create<IUserRepository>();
        }

        public void Dispose() => _repository.VerifyAll();

        private SignInCommandHandler CreateHandler()
        {
            return new SignInCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_PasswordError_Should_Throws_UserDomainException()
        {
            // arrange
            var command = Factory.CreateTestSignInCommand();
            var user = Factory.CreateTestApplicationUser(password: command.Password + "random");
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s.Equals(command.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user)
                .Verifiable();
            var handler = CreateHandler();

            // act
            var exception = await Assert.ThrowsAsync<UserDomainException>(() => handler.Handle(command, default));

            // assert
            Assert.Equal(UserErrorCodes.SignInError, exception.ErrorCode);
        }
        [Fact]
        public async Task Handle_UserIsNull_Should_Throws_UserDomainException()
        {
            // arrange
            var command = Factory.CreateTestSignInCommand();
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s.Equals(command.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(ApplicationUser))
                .Verifiable();
            var handler = CreateHandler();

            // act
            var exception = await Assert.ThrowsAsync<UserDomainException>(() => handler.Handle(command, default));

            // assert
            Assert.Equal(UserErrorCodes.SignInError, exception.ErrorCode);
        }

        [Fact]
        public async Task Handle_StatusIsBlocked_Throws_UserDomainException()
        {
            // arrange
            var command = Factory.CreateTestSignInCommand();
            var user = Factory.CreateTestApplicationUser(password: command.Password, status: UserStatus.Blocked);
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s.Equals(command.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user)
                .Verifiable();
            var handler = CreateHandler();

            // act
            var exception = await Assert.ThrowsAsync<UserDomainException>(() => handler.Handle(command, default));

            // assert
            Assert.Equal(UserErrorCodes.StatusError, exception.ErrorCode);
        }

        [Fact]
        public async Task Handle_Return_UserDto()
        {
            // arrange
            var command = Factory.CreateTestSignInCommand();
            var user = Factory.CreateTestApplicationUser(password: command.Password);
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(s => s.Equals(command.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user)
                .Verifiable();
            var handler = CreateHandler();

            // act
            var dto = await handler.Handle(command, default);

            // assert
            Assert.NotNull(dto);
            Assert.Equal(user.UserName, dto.UserName);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Status, dto.Status);
        }
    }
}
