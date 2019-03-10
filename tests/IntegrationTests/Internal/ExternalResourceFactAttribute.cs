using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace IntegrationTests.Internal
{
    public class ExternalResourceFactAttribute : FactAttribute
    {


        private bool TryAccessPostgresql(out string message)
        {
            message = "Postgresql server is connectable";

            if (TestConfig.ConnectionString == null)
            {
                message = $"{nameof(TestConfig)}.{nameof(TestConfig.ConnectionString)} are not configured";

                return false;
            }

            try
            {
                using (var connection = new NpgsqlConnection(TestConfig.ConnectionString))
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return false;
            }

            return true;
        }

        private bool TryAccessRabbitMQ(out string message)
        {
            message = "RabbitMQ server is connectable";

            if (TestConfig.ConnectionFactory == null)
            {
                message = $"{nameof(TestConfig)}.{nameof(TestConfig.ConnectionFactory)} are not configured";

                return false;
            }

            try
            {
                using (TestConfig.ConnectionFactory.CreateConnection())
                {

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return false;
            }

            return true;
        }

        public ExternalResourceFactAttribute(params ExternalResource[] resources)
        {
            if (Skip == null && resources.Contains(ExternalResource.Postgresql))
            {
                Skip = TryAccessPostgresql(out var failureMessage) ? null : GetFormatedSkipReason(ExternalResource.Postgresql, failureMessage);
            }

            if (Skip == null && resources.Contains(ExternalResource.RabbitMQ))
            {
                Skip = TryAccessRabbitMQ(out var failureMessage) ? null : GetFormatedSkipReason(ExternalResource.RabbitMQ, failureMessage);
            }
        }

        private string GetFormatedSkipReason(ExternalResource resource, string detail)
        {
            return "Disabled due to CI failure." +
                "This test required access " + resource.ToString() + " in runner agent machine." +
               Environment.NewLine +
                "Skip Detail:" +
                detail;
        }
    }
}
