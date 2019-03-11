using Identity.Api;
using Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

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

        public static IdentityServiceContext EnsureTestDatabaseCreated(string connectionString)
        {
            var container = new ServiceCollection()
                .AddDbContext<IdentityServiceContext>(options => {
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
