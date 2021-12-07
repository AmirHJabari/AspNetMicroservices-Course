using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotApiGw
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IWebHostEnvironment env)
        {
            this._env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var ocelotConfigs = new ConfigurationBuilder()
                                        .AddJsonFile($"ocelot.json", true, true)
                                        .AddJsonFile($"ocelot.{_env.EnvironmentName}.json", true, true)
                                        .Build();
            
            services.AddOcelot(ocelotConfigs)
                .AddCacheManager(c => c.WithDictionaryHandle());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();
        }
    }
}
