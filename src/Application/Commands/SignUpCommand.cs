using Application.Models;
using MediatR;

namespace Application.Commands
{
    public sealed class SignUpCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
