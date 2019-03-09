using System;

namespace Infrastructure
{
    public class InfraOptions
    {
        public string ConnectionString { get; set; }

        public RabbitMQOptionBuilder RabbitMQ { get; } = new RabbitMQOptionBuilder();

        public AdministrastorProfile AdministrastorSettings { get; } = new AdministrastorProfile();

        public class AdministrastorProfile
        {
            public string UserName { get; set; }

            public string Password { get; set; }
        }

        public class RabbitMQOptionBuilder
        {
            public string Host { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }

            public int? Port { get; set; }

            public string MessageVersion { get; set; }
            public string QueueName { get; set; }
        }
    }
}
