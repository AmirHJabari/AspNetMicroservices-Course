using EventBus.Messages.Events;
using EventBus.Messages.Common;
using Ordering.Application;
using Ordering.Infrastructure;
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
using Ordering.API.Extensions;
using Ordering.Infrastructure.Persistence;
using System.Threading;
using MassTransit;
using Ordering.API.EventBusConsumers;

namespace Ordering.API
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
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            services.AddAutoMapper(typeof(Program));

            #region Add MassTransit and RabbitMq
            var massTransitSettings = Configuration.GetSection("MassTransitSettings").Get<MassTransitSettings>();
            services.AddSingleton<IMassTransitSettings>(massTransitSettings);

            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketCheckoutConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(massTransitSettings.RabbitMqHostUrl);
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, configEndpoint =>
                    {
                        configEndpoint.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
            #endregion

            services.AddControllers();

            if (IsSwaggerOn)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDocker()) // Wait for sqlserver container to start and run completely
                Thread.Sleep(15 * 1000);

            using (var scope = app.ApplicationServices.CreateScope())
                Initialize(scope.ServiceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IsSwaggerOn)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
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
            services.MigrateDatabase<OrderDbContext>((db, services) =>
                {
                    var logger = services.GetRequiredService<ILogger<SeedOrderContext>>();
                    SeedOrderContext.SeedAsync(db, logger).Wait();
                },
                40);

            #endregion
        }
    }
}
