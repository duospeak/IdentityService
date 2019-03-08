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
    [ApiController]
    [Route("user/manager")]
    public class UserManagerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _queries;

        public UserManagerController(IMediator mediator, IUserQueries queries)
        {
            _mediator = mediator.NotNull(nameof(mediator));
            _queries = queries.NotNull(nameof(mediator));
        }

        [HttpGet]
        public async Task<ActionResult<UserListDto[]>> Get(int offset = 0, int count = 20)
        {
            count = count > 200 ? 200 : count;
            count = count < 1 ? 1 : count;
            offset = offset < 0 ? 0 : offset;
            var managers = await _queries.QueryManagersAsync(offset, count, HttpContext.RequestAborted);

            return managers.ToArray();
        }
       
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody]SignUpCommand command)
        {
            var user = await _mediator.Send(command, HttpContext.RequestAborted);

            return user;
        }

        [HttpGet("status/{id}")]
        public async Task<ActionResult<UserStatus>> Status(long id)
        {
            var status = await _queries.QueryStatusByUserIdAsync(id, HttpContext.RequestAborted);

            return status;
        }


    }
}
