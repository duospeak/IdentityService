using Application.Commands;
using Application.Models;
using Application.Queries;
using Identity.Api.Controllers;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Internal;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class UserControllerTests : IDisposable
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserQueries> _queriesMock;
        //private readonly Mock<ISystemClock> _clockMock;
        public UserControllerTests()
        {
            _repository = new MockRepository(MockBehavior.Strict);

            _mediatorMock = _repository.Create<IMediator>();
            _queriesMock = _repository.Create<IUserQueries>();
            //_clockMock = _repository.Create<ISystemClock>();
        }

        private readonly MockRepository _repository;
        public void Dispose() => _repository.VerifyAll();

        private UserController CreateController()
        {
            return new UserController(_mediatorMock.Object, _queriesMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
        [Fact]
        public async Task SignIn_Should_Return_SignInResult()
        {
            var requiredClaims = new[]
            {
               "name",
               "Status",
                "sub",
                "auth_time"
            };

            // arrange
            var parameter = new SignInCommand();
            var result = Factory.CreateTestUserDto();
            _mediatorMock.Setup(x => x.Send(It.Is<SignInCommand>(p => ReferenceEquals(p, parameter)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result)
                .Verifiable();
            var controller = CreateController();

            // act
            var response = await controller.SignIn(parameter);

            // assert
            Assert.NotNull(response);
            Assert.NotNull(response.Principal);
            Assert.NotNull(response.Principal.Claims);
            Assert.Equal(response?.AuthenticationScheme, IdentityServerConstants.DefaultCookieAuthenticationScheme);
            Assert.Subset(response?.Principal?.Claims?.Select(x => x.Type).ToHashSet(), requiredClaims.ToHashSet());
        }
        [Fact]
        public async Task SignUp_Should_Call_Mediator_And_Return_User()
        {
            // arrange
            var parameter = new SignUpCommand();
            var result = Factory.CreateTestUserDto();
            _mediatorMock.Setup(x => x.Send(It.Is<SignUpCommand>(p => ReferenceEquals(p, parameter)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result)
                .Verifiable();
            var controller = CreateController();

            // act
            var response = await controller.Post(parameter);

            // assert
            Assert.NotNull(response);
            Assert.Same(response.Value, result);
        }
    }
}
