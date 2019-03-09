using Docker.DotNet;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    internal static class DockerExtensions
    {
        public static async Task<ContainerListResponse> GetContainerAsync(this DockerClient client, string name)
        {
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters
            {
                All = true
            });

            return containers.SingleOrDefault(x => x.Names.Contains(name));
        }

        public static bool IsRunning(this ContainerListResponse container)
            => string.Equals(container.State, "running", System.StringComparison.InvariantCultureIgnoreCase);

        public static async Task<CreateContainerResponse> CreateContainerAsync(this DockerClient client, string image, string name, string portMap, params string[] envs)
        {
            var container = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = image,
                Name = name,
                Env = envs,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { portMap , default }
                }
            });

            return container;
        }

        public static async Task<bool> ContainsImageAsync(this DockerClient client,string image)
        {
            var images = await client.Images.ListImagesAsync(new ImagesListParameters());

            return images.Any(i => i.RepoTags.Contains(image));
        }
    }
}
