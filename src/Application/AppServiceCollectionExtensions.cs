using Application.Queries;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppServiceCollectionExtensions
    {
        public static IServiceCollection AddApp(this IServiceCollection services,Action<InfraOptions> optionAction)
        {
            services.NotNull(nameof(services));
            optionAction.NotNull(nameof(optionAction));

            services.AddInfra(optionAction);

            services.AddMediatR();

            //services.TryAddSingleton<IUserQueries, UserQueries>();

            return services;
        }
    }
}
