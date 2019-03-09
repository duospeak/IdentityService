using Dapper;
using Infrastructure.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Internal
{
    internal class DbConfiguredOption : IConfigureOptions<IdentityServiceOptions>
    {
        private readonly InfraOptions _options;

        private IdentityServiceOptions _existingOptions;
        public DbConfiguredOption(IOptions<InfraOptions> options)
        {
            _options = options.NotNull(nameof(options)).Value;
        }

        public void Configure(IdentityServiceOptions options)
        {
            if (_existingOptions == null)
            {
                using (var connection = new NpgsqlConnection(_options.ConnectionString))
                {
                    _existingOptions = connection.QueryFirstOrDefault<IdentityServiceOptions>(GetPrepareSql());
                }
            }
            
            if (_existingOptions != null)
            {
                options.AdminitrastorAccpetFlow = _existingOptions.AdminitrastorAccpetFlow;
                options.EmailVerificationFlow = _existingOptions.EmailVerificationFlow;
                options.PhoneVerificationFlow = _existingOptions.PhoneVerificationFlow;
            }
        }

        private string GetPrepareSql()
        {
            return "SELECT * FROM `ServiceOptions` WHERE `Enable` = TURE";
        }
    }
}
