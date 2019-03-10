using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class UserDto
    {
        public UserStatus Status { get; set; }

        public long Id { get; set; }

        public string UserName { get; set; }
        
    }
}
