using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Core.Data;
using Discount.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Discount.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool IsSwaggerOn => Configuration.GetValue<bool>("IsSwaggerOn");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPostgreSql(Configuration);

            services.AddControllers();

            if (IsSwaggerOn)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Discount.API", Version = "v1" });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
                Initialize(scope.ServiceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IsSwaggerOn)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Initilizes the application requirements.
        /// </summary>
        /// <param name="services">The required services.</param>
        private void Initialize(IServiceProvider services)
        {
            services.MigratePostgreSql<Program>(10);
        }
    }
}
