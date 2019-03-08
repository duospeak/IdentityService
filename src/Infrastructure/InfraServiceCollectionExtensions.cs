using Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class InfraServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services,Action<InfraOptions> optionAction)
        {
            services.NotNull(nameof(services));
            services.NotNull(nameof(optionAction));

            services.Configure(optionAction);

            services.TryAddEnumerable(ServiceDescriptor.Scoped<IConfigureOptions<IdentityServiceOptions>, DbConfiguredOption>());

            return services;
        }
    }
}
