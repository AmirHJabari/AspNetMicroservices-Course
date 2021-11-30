using Catalog.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool IsSwaggerOn => Configuration.GetValue<bool>("IsSwaggerOn");

        public void ConfigureServices(IServiceCollection services)
        {
            var mongoSettings = Configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton<IMongoSettings>(mongoSettings);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogContext, CatalogContext>();

            services.AddControllers();

            if (IsSwaggerOn)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
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
            #region Seed Data
            var catalogContext = services.GetRequiredService<ICatalogContext>();
            SeedCatalogContext.SeedPrudocts(catalogContext.Products);
            #endregion
        }
    }
}
