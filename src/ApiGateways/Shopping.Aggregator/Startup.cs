using Shopping.Aggregator.HttpClients;
using Shopping.Aggregator.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Settings;
using AutoMapper;

namespace Shopping.Aggregator
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
            var urlsSettings = Configuration.GetSection(nameof(UrlsSettings)).Get<UrlsSettings>();
            services.AddSingleton<IUrlsSettings>(urlsSettings);

            services.AddHttpClient<ICatalogClient, CatalogClient>(c =>
            {
                c.BaseAddress = new(urlsSettings.CatalogBaseUrl);
            });
            services.AddHttpClient<IBasketClient, BasketClient>(c =>
            {
                c.BaseAddress = new(urlsSettings.BasketBaseUrl);
            });
            services.AddHttpClient<IOrderingClient, OrderingClient>(c =>
            {
                c.BaseAddress = new(urlsSettings.OrderingBaseUrl);
            });

            services.AddAutoMapper(typeof(Program));

            services.AddControllers();
            if (IsSwaggerOn)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IsSwaggerOn)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping.Aggregator v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
