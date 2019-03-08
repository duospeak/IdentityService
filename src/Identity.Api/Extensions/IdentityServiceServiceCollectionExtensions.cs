using FluentValidation.AspNetCore;
using Identity.Api.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IdentityServiceServiceCollectionExtensions
    {


        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.NotNull(nameof(services));

            var builder = services.AddMvcCore(options =>
            {
                //options.Filters.Add(null);
            });

            // for docs
            builder.AddApiExplorer();

            builder.AddJsonFormatters(options =>
            {
                options.Converters.Add(new StringEnumConverter());
            });

            builder.AddFormatterMappings();

            builder.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssembly(typeof(IdentityServiceServiceCollectionExtensions).Assembly);
            });

            builder.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.Policy1, policy =>
                {
                    policy.RequireAuthenticatedUser().RequireClaim("scope", ".user.delete");
                });
            });

            builder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services;
        }


        public static IServiceCollection AddIdentityServerServices(this IServiceCollection services, string issuerUri, Action<DbContextOptionsBuilder> optionAction)
        {
            services.NotNull(nameof(services));
            optionAction.NotNull(nameof(optionAction));

            var builder = services.AddIdentityServer(options =>
            {
                options.IssuerUri = issuerUri;
            });

            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = optionAction;
            });

            builder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = optionAction;
            });

            builder.AddDeveloperSigningCredential();

            var securityKeySource = JsonConvert.DeserializeAnonymousType(File.ReadAllText("devkey.rsa"), new
            {
                keyId = default(string),
                parameters = default(RSAParameters)
            });

            var securityKey = new RsaSecurityKey(securityKeySource.parameters);

            securityKey.KeyId = securityKeySource.keyId;

            builder.AddSigningCredential(securityKey);

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.IssuerSigningKey = securityKey;
                    options.TokenValidationParameters.ValidIssuer = "https://supplychain.com";
                    options.Audience = "identityservice";
                });

            return services;
        }
    }
}
