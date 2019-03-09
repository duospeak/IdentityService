using MediatR;
using System.Security.Claims;

namespace Application.Commands
{
    public class SignInCommand : IRequest<ClaimsPrincipal>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
