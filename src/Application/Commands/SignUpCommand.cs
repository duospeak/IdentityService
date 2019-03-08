using Application.Models;
using Domain.Enumerations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands
{
    public sealed class SignUpCommand:IRequest<UserDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
