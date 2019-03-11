using Application.Commands;
using Domain.AggregatesModel;
using Domain.Exceptions;
using Domain.SeedWork;
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
    public class SignUpCommandTests : IDisposable
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly MockRepository _repository;

        public SignUpCommandTests()
        {
            _repository = new MockRepository(MockBehavior.Strict);
            _repositoryMock = _repository.Create<IUserRepository>();
        }

        public void Dispose() => _repository.VerifyAll();

        private SignUpCommandHandler CreateHandler()
        {
            return new SignUpCommandHandler(_repositoryMock.Object);
        }
        [Fact]
        public async Task Handle_UserNotNull_ThrowsDomainException()
        {
            var parameters = Factory.CreateTestSignUpCommand();
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(p1 => string.Equals(p1, parameters.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Factory.CreateTestApplicationUser())
                .Verifiable();
            var handler = CreateHandler();

            var exception = await Assert.ThrowsAsync<UserDomainException>(() => handler.Handle(parameters, default));

            Assert.Equal(exception.ErrorCode, UserErrorCodes.SignedUp);
        }
        [Fact]
        public async Task Handle_UserIsNull_Ok()
        {
            var parameters = Factory.CreateTestSignUpCommand();
            _repositoryMock.Setup(x => x.GetAsync(It.Is<string>(p1 => string.Equals(p1, parameters.UserName)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(ApplicationUser))
                .Verifiable();
            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockReturnDefaults<ApplicationUser>.ItselfReturns)
                .Verifiable();

            var unitofwork = _repository.Create<IUnitOfWork>();
            unitofwork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1)
                .Verifiable();
            _repositoryMock.SetupGet(x => x.UnitOfWork)
                .Returns(unitofwork.Object)
                .Verifiable();

            var handler = CreateHandler();

            var result = await handler.Handle(parameters, default);

            Assert.NotNull(result);
        }
    }
}
