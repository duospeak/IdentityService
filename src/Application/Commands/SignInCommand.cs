using Application.Models;
using MediatR;
using System.Security.Claims;

namespace Application.Commands
{
    public class SignInCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
