using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class IdentityServiceOptions
    {
        public long Id { get; set; }
        public bool EmailVerificationFlow { get; set; }

        public bool PhoneVerificationFlow { get; set; }

        public bool AdminitrastorAccpetFlow { get; set; }
    }
}
