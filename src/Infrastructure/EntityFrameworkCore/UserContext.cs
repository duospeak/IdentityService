using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EntityFrameworkCore
{
    public class UserContext:DbContext
    {
        public DbSet<IdentityServiceOptions> ServiceOptions { get; set; }
    }
}
