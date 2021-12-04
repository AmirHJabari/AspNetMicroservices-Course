using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Grpc;
using Basket.API.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Basket.API
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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = Configuration.GetValue<string>("RedisInstanceName");
            });

            services.AddAutoMapper(typeof(Program));

            #region Add Grpc
            var grpcSettings = Configuration.GetSection("GrpcSettings").Get<GrpcSettings>();
            services.AddSingleton<IGrpcSettings>(grpcSettings);

            services.AddGrpcClient<Discount.Grpc.Protos.Discount.DiscountClient>(options =>
                        options.Address = new Uri(grpcSettings.DiscountGrpcUrl)
            );
            services.AddScoped<IDiscountGrpcClient, DiscountGrpcClient>();
            #endregion

            #region Add MassTransit and RabbitMq
            var massTransitSettings = Configuration.GetSection("MassTransitSettings").Get<MassTransitSettings>();
            services.AddSingleton<IMassTransitSettings>(massTransitSettings);

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(massTransitSettings.RabbitMqHostUrl);
                });
            });
            services.AddMassTransitHostedService();
            #endregion

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddControllers();

            if (IsSwaggerOn)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
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
