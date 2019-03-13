using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain.Enumerations;
using Identity.Api.Extensions;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Api.Controllers
{
    /// <summary>
    /// User common apis
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _queries;
        /// <summary>
        /// create new user
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="queries"></param>
        public UserController(IMediator mediator, IUserQueries queries)
        {
            _mediator = mediator.NotNull(nameof(mediator));
            _queries = queries.NotNull(nameof(mediator));
        }
        /// <summary>
        /// User sign up
        /// </summary>
        /// <param name="command">request parameters</param>
        /// <returns>The user created</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> SignUp([FromBody]SignUpCommand command)
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
        public async Task<ActionResult<UserStatus?>> Status(long id)
        {
            var status = await _queries.QueryStatusByUserIdAsync(id, HttpContext.RequestAborted);

            return status;
        }
        /// <summary>
        /// Create and start user session
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("session")]
        public async Task<SignInResult> SignIn([FromBody]SignInCommand command)
        {
            var user = await _mediator.Send(command, HttpContext.RequestAborted);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = command.RememberMe,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            return SignIn(user.ToPrincipal(), properties, IdentityServerConstants.DefaultCookieAuthenticationScheme);
        }
        /// <summary>
        /// Remove the user session
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("session")]
        public SignOutResult SignOut()
            => SignOut(IdentityServerConstants.DefaultCookieAuthenticationScheme);
    }
}
