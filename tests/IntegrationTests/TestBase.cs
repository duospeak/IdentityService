using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestBase
    {
        public static readonly NpgsqlConnectionStringBuilder ConnectionString;
        public static readonly DockerClient Docker;
        static TestBase()
        {
            Docker = new DockerClientConfiguration(new Uri("tcp://localhost:2375"))
                .CreateClient();

            ConnectionString = new NpgsqlConnectionStringBuilder
            {
                Port = 8080,
                Password = "123456",
                Username = "postgres",
                Host = "localhost"
            };
        }

        protected async Task<NpgsqlConnectionStringBuilder> StartNpgsqlServer()
        {

            var postgresql = await Docker.GetContainerAsync("xunit-postgresql-server");

            if (postgresql == null)
            {
                if (!await Docker.ContainsImageAsync("postgres:latest"))
                {
                    //Docker.("postgres:latest")
                }

                await Docker.CreateContainerAsync("postgres",
                    "xunit-postgresql-server",
                    $"{ConnectionString.Port}:8080",
                    $"POSTGRES_USER={ConnectionString.Username}",
                    $"POSTGRES_PASSWORD={ConnectionString.Password}");
            }

            return ConnectionString;
        }

        private async Task<bool> IsContainerRunning(string name)
        {
            var containers = await Docker.Containers.ListContainersAsync(new ContainersListParameters
            {
                All = true
            });

            return containers.Any(x => string.Equals(x.State, "running", StringComparison.InvariantCultureIgnoreCase) && x.Names.Contains(name));
        }


    }
}
