using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain.Enumerations;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("user/manager")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _queries;

        public UserController(IMediator mediator, IUserQueries queries)
        {
            _mediator = mediator.NotNull(nameof(mediator));
            _queries = queries.NotNull(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody]SignUpCommand command)
        {
            var user = await _mediator.Send(command, HttpContext.RequestAborted);

            return user;
        }

        /// <summary>
        /// Check user status
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<ActionResult<UserStatus>> Status(long id)
        {
            var status = await _queries.QueryStatusByUserIdAsync(id, HttpContext.RequestAborted);

            return status;
        }

        [HttpPost("cookie")]
        public async Task<SignInResult> SignIn([FromBody]SignInCommand command)
        {
            var principal = await _mediator.Send(command, HttpContext.RequestAborted);

            return SignIn(principal, IdentityServerConstants.DefaultCookieAuthenticationScheme);
        }

        [Authorize]
        public SignOutResult SignOut() 
            => SignOut(IdentityServerConstants.DefaultCookieAuthenticationScheme);
    }
}
