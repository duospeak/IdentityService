using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class InfraOptions
    {
        public string ConnectionString { get; set; }

        public AdministrastorProfile AdministrastorSettings { get; } = new AdministrastorProfile();

        public class AdministrastorProfile
        {
            public string UserName { get; set; }

            public string Password { get; set; }
        }
    }
}
