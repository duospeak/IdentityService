using Identity.Api;
using Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace IntegrationTests.Internal
{
    public static class TestConfig
    {
        public static readonly string ConnectionString;
        public static readonly ConnectionFactory ConnectionFactory;
        /// <summary>
        /// Get available port that can listen,if no port are available,returns -1;
        /// </summary>
        /// <returns></returns>
        public static int GetAvailablePort()
        {
            var endpoints = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

            var result = 0;

            for (var port = 1000; port < IPEndPoint.MaxPort; port++)
            {
                result = endpoints.Any(x => x.Port == port) ? -1 : port;
            }

            return result;
        }

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

        public static IdentityServiceContext EnsureTestDatabaseCreated(string connectionString)
        {
            var container = new ServiceCollection()
                .AddDbContext<IdentityServiceContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                })
                .AddCap(options =>
                {
                    options.UsePostgreSql(connectionString);
                }).Services.BuildServiceProvider(false);

            using (container)
            {
                return null;
            }

            //using(var context=)
        }
    }
}
