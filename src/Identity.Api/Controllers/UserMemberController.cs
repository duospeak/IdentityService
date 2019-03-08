using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity.Api.Controllers
{
    [Route("user/member")]
    [ApiController]
    public class UserMemberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _queries;

        public UserMemberController(IMediator mediator, IUserQueries queries)
        {
            _mediator = mediator.NotNull(nameof(mediator));
            _queries = queries.NotNull(nameof(mediator));
        }

        /// <summary>
        /// Get all members
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<UserListDto[]>> Get(int offset = 0, int count = 20)
        {
            count = count > 200 ? 200 : count;
            count = count < 1 ? 1 : count;
            offset = offset < 0 ? 0 : offset;
            var users = await _queries.QueryMemberUsersAsync(offset, count, HttpContext.RequestAborted);

            return users.ToArray();
        }
        /// <summary>
        /// Get blocked members
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("blocked")]
        public async Task<ActionResult<UserListDto[]>> Blocked(int offset = 0, int count = 20)
        {
            count = count > 200 ? 200 : count;
            count = count < 1 ? 1 : count;
            offset = offset < 0 ? 0 : offset;
            var users = await _queries.QueryBlockedMemberUsersAsync(offset, count, HttpContext.RequestAborted);

            return users.ToArray();
        }
        /// <summary>
        /// Get actived members
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("members/actived")]
        public async Task<ActionResult<UserListDto[]>> Actived(int offset = 0, int count = 20)
        {
            count = count > 200 ? 200 : count;
            count = count < 1 ? 1 : count;
            offset = offset < 0 ? 0 : offset;
            var users = await _queries.QueryActivedMemberUsersAsync(offset, count, HttpContext.RequestAborted);

            return users.ToArray();
        }
        /// <summary>
        /// Sign up a new member
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody]SignUpCommand command)
        {
            var user = await _mediator.Send(command, HttpContext.RequestAborted);

            return user;
        }
        /// <summary>
        /// Get member details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            var user = await _queries.QueryMemberUserByIdAsync(id, HttpContext.RequestAborted);

            return user;
        }


    }
}
