using System.Diagnostics;
using Xunit;

namespace IntegrationTests
{
    /// <summary>
    /// Attribute that is applied to a method to indicate that docker should be running before xunit runner execute test method.
    /// </summary>
    public class DockerFactAttribute : FactAttribute
    {
        public bool IsDockerRunning()
        {
            var docker = Process.Start("docker", "ps");

            docker.WaitForExit();

            return docker.ExitCode == 0;
        }


        public DockerFactAttribute() => Skip = IsDockerRunning() ? null : IntegrationTestConstants.SkipReason;
    }
}
