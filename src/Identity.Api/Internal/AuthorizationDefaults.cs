using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Internal
{
    internal static class AuthorizationDefaults
    {
        public static readonly AuthorizationPolicy ReadUser;

        public static IEnumerable<AuthorizationPolicy> GetPolicies()
        {
            yield return ReadUser;
        }
    }

    internal struct AuthorizationPolicy
    {
        public string Name { get; }

        public string[] Scopes { get; }
    }
}
