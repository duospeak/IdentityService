using Application.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.Extensions
{
    internal static class DtoExtensions
    {
        public static ClaimsPrincipal ToPrincipal(this UserDto user)
        {
            var identityServerUser = new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.UserName,
                AuthenticationTime = DateTimeOffset.UtcNow.UtcDateTime,
                AdditionalClaims = GetUserClaims(user).ToHashSet()
            };

            return identityServerUser.CreatePrincipal();
        }

        private static IEnumerable<Claim> GetUserClaims(UserDto user)
        {
            yield return new Claim(nameof(user.Status), user.Status.ToString());
        }
    }
}
