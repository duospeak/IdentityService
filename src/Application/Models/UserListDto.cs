using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
   public  class UserListDto
    {
        public string UserName { get; set; }

        public UserStatus Status { get; set; }
    }
}
