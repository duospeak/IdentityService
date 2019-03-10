using Application.Models;
using Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    internal static class DomainEntitiesExtensions
    {
        public static UserDto AsDto(this ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Status = user.Status,
                UserName = user.UserName
            };
        }
    }
}
