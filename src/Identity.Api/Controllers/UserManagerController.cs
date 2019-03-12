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
    /// <summary>
    /// Adminitrastor user apis
    /// </summary>
    [ApiController]
    [Route("user/manager")]
    public class UserManagerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _queries;
        /// <summary>
        /// Initial
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="queries"></param>
        public UserManagerController(IMediator mediator, IUserQueries queries)
        {
            _mediator = mediator.NotNull(nameof(mediator));
            _queries = queries.NotNull(nameof(mediator));
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<UserListDto[]>> Managers(int offset = 0, int count = 20)
        {
            count = count > 200 ? 200 : count;
            count = count < 1 ? 1 : count;
            offset = offset < 0 ? 0 : offset;
            var managers = await _queries.QueryManagersAsync(offset, count, HttpContext.RequestAborted);

            return managers.ToArray();
        }
    }
}
