using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Identity.Api
{
    /// <summary>
    /// Startup for configure the service
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initial
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration.NotNull(nameof(configuration));
        }


        /// <summary>
        /// The application configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi();

            var issuerUri = Configuration["IssuerUri"];

            services.AddIdentityServerServices(issuerUri, ConfigureId4Context);

            //services.AddApp(options =>
            //{
            //    options.ConnectionString = Configuration.GetConnectionString("Master");
            //    options.RabbitMQHost = Configuration["RabbitMQHost"];
            //});

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("identityservice", CreateIdentityServiceOpenApiInfo("v1"));

                Array.ForEach(
                    Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories),
                    file => options.IncludeXmlComments(file));

                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIdentityServer();

            app.UseMvc();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "{documentName}/open_api.json";
            });
        }
        /// <summary>
        /// Configure the identityserver4 dbcontexts.
        /// </summary>
        /// <param name="options"></param>
        protected virtual void ConfigureId4Context(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("IdentityService");
        }

        private OpenApiInfo CreateIdentityServiceOpenApiInfo(string version)
        {
            return new OpenApiInfo
            {
                Contact = new OpenApiContact()
                {
                    Email = "1015450578@qq.com",
                    Name = "opensupplychain",
                    Url = new Uri("https://github.com/opensupplychain")
                },
                Description = "Open source identity server to protect APIs and other Microservices",
                License = new OpenApiLicense()
                {
                    Url = new Uri("https://github.com/opensupplychain/IdentityService/blob/master/LICENSE"),
                    Name = "MIT License"
                },
                Title = "Identity Service",
                Version = version
            };
        }
    }
}
