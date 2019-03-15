using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Api.Internal
{
    // SecurityRequirementsOperationFilter.cs
    internal class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security.Add(GetSecurity(requiredScopes));
            }
        }

        private OpenApiSecurityRequirement GetSecurity(IEnumerable<string> requiredScopes)
        {
            var requirements = new OpenApiSecurityRequirement();

            var scopes = IdentityServiceScopes
                .ScopeDescriptions
                .Where(x => requiredScopes.Contains(x.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var oauth2 = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            };

            //var jwt = new OpenApiSecurityScheme
            //{
            //    Description = "IdentityService Jwt",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.Http
            //};

            requirements.Add(oauth2, requiredScopes.ToList());

            //requirements.Add(jwt, requiredScopes.ToList());

            return requirements;
        }
    }
}
