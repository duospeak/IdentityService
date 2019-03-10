using Infrastructure;
using Infrastructure.EntityFrameworkCore;
using Infrastructure.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, Action<InfraOptions> optionAction)
        {
            services.NotNull(nameof(services));
            services.NotNull(nameof(optionAction));

            services.Configure(optionAction);

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IConfigureOptions<IdentityServiceOptions>, DbConfiguredOption>());

            services.AddCap(options =>
            {
                var source = new InfraOptions();
                optionAction(source);

                options.Version = source.RabbitMQ.MessageVersion ?? options.Version;
                options.DefaultGroup = source.RabbitMQ.QueueName ?? options.DefaultGroup;
                options.UseRabbitMQ(rabbitmq =>
                {
                    rabbitmq.UserName = source.RabbitMQ.UserName ?? rabbitmq.UserName;
                    rabbitmq.Password = source.RabbitMQ.Password ?? rabbitmq.Password;
                    rabbitmq.HostName = source.RabbitMQ.Host ?? rabbitmq.HostName;
                    rabbitmq.Port = source.RabbitMQ.Port ?? rabbitmq.Port;
                });

                options.UsePostgreSql(source.ConnectionString);
            });

            services.AddDbContext<IdentityServiceContext>((sp, options) =>
            {
                var source = sp.GetRequiredService<IOptions<InfraOptions>>().Value;
                options.UseNpgsql(source.ConnectionString, npgsql =>
                {
                    npgsql.MigrationsAssembly(Assembly.GetEntryAssembly().GetName().Name);
                });
            });

            return services;
        }
    }
}
