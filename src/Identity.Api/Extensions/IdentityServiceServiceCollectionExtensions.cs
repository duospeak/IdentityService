using FluentValidation.AspNetCore;
using Identity.Api.Internal;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

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
                IdentityServiceScopes.ScopeDescriptions.ToList().ForEach(scope =>
                {
                    options.AddPolicy(scope.Key, policy =>
                    {
                        policy.RequireClaim("scope", scope.Key);
                    });
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

            var securityKeySource = JsonConvert.DeserializeAnonymousType(File.ReadAllText("devkey.rsa"), new
            {
                keyId = default(string),
                parameters = default(RSAParameters)
            });

            var securityKey = new RsaSecurityKey(securityKeySource.parameters)
            {
                KeyId = securityKeySource.keyId
            };

            builder.AddSigningCredential(securityKey);

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.IssuerSigningKey = securityKey;
                    options.TokenValidationParameters.ValidIssuer = issuerUri;
                    options.Audience = "identityservice";
                });

            return services;
        }
    }
}
