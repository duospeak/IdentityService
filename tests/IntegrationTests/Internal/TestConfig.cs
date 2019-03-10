using Identity.Api;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Internal
{
    public static class TestConfig
    {
        public static readonly string ConnectionString;
        public static readonly ConnectionFactory ConnectionFactory;

        static TestConfig()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Program).Assembly)
                .AddEnvironmentVariables()
                //.AddJsonFile("testsettings.json")
                .Build();

            ConnectionString = config.GetConnectionString("IntegrationTests");

            var host = config.GetSection("RabbitMQ")["Host"];

            if (host != null)
            {
                ConnectionFactory = new ConnectionFactory()
                {
                    HostName = host
                };
            }
        }
    }
}
