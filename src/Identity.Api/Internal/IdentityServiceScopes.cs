using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Internal
{
    /// <summary>
    /// The identity service available scopes
    /// </summary>
    internal static class IdentityServiceScopes
    {
        static IdentityServiceScopes()
        {
            ScopeDescriptions = new Dictionary<string, string>();

            ScopeDescriptions.Add(WriteUserStatus, "block/active user permission");
        }
        public static readonly Dictionary<string, string> ScopeDescriptions;
        public const string WriteUserStatus = "user_status_write";
    }
}
