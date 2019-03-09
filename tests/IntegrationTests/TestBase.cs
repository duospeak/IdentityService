using Docker.DotNet;
using Docker.DotNet.Models;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestBase
    {
        public static readonly DockerClient Docker;
        public static readonly string RunningDockerContainersFile = "docker-containers.json";
        static TestBase()
        {
            Docker = new DockerClientConfiguration(new Uri("tcp://localhost:2375"))
                .CreateClient();
        }

        protected virtual async Task<NpgsqlConnectionStringBuilder> CreateNpgSqlServerContainerIfNotExistAsync()
        {
            var postgresql = GetOrAddContainerFromFile("postgres");

            var container = await GetOrStartContainer(postgresql);

            return null;
        }

        private RunningContainer GetOrAddContainerFromFile(string image)
        {
            List<RunningContainer> existingContainers;


            if (File.Exists(RunningDockerContainersFile))
            {
                existingContainers = JsonConvert.DeserializeObject<List<RunningContainer>>(File.ReadAllText(RunningDockerContainersFile));
            }
            else
            {
                File.Create(RunningDockerContainersFile);
                existingContainers = new List<RunningContainer>();
            }

            var result = existingContainers.SingleOrDefault(x => x.Image == image);

            if (result == null)
            {
                result = new RunningContainer()
                {
                    Id = Guid.NewGuid().ToString(),
                    Image = image
                };

                existingContainers.Add(result);

                File.WriteAllText(RunningDockerContainersFile, JsonConvert.SerializeObject(existingContainers));
            }

            return result;
        }

        private async Task<ContainerListResponse> GetOrStartContainer(RunningContainer container)
        {
            var existingContainers = await Docker.Containers.ListContainersAsync(new ContainersListParameters { All = true });

            var result = existingContainers.SingleOrDefault(x => x.Image == container.Image);

            switch (result?.Status)
            {
                case "Up":
                    break;
                default:
                    await Docker.Containers.CreateContainerAsync(new CreateContainerParameters()
                    {
                        Image = container.Image,
                        Env = container.Env
                    });

                    break;
            }

            return result;
        }

        private class RunningContainer
        {
            public string Id { get; set; }

            public string Image { get; set; }

            public List<string> Env { get; set; }
        }
    }
}
