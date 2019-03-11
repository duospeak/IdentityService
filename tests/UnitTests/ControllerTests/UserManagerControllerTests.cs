using Application.Queries;
using Identity.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.ControllerTests
{
    public class UserManagerControllerTests : IDisposable
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUserQueries> _queriesMock;
        public UserManagerControllerTests()
        {

        }

        public void Dispose() => throw new NotImplementedException();

        private UserManagerController CreateController()
        {
            return new UserManagerController(_mediator.Object, _queriesMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
    }
}
