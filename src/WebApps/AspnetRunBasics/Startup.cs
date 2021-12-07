using AspnetRunBasics.HttpClients;
using AspnetRunBasics.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspnetRunBasics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiSettings = Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();
            services.AddSingleton<IApiSettings>(apiSettings);

            services.AddHttpClient<ICatalogClient, CatalogClient>(c =>
            {
                c.BaseAddress = new(apiSettings.BaseUrl);
            });
            services.AddHttpClient<IBasketClient, BasketClient>(c =>
            {
                c.BaseAddress = new(apiSettings.BaseUrl);
            });
            services.AddHttpClient<IOrderingClient, OrderingClient>(c =>
            {
                c.BaseAddress = new(apiSettings.BaseUrl);
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
