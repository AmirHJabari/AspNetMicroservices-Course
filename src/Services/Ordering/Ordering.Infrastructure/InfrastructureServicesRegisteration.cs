using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Email;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration, Action<InfrastructureDiOptions> optionsBuilder = null)
        {
            InfrastructureDiOptions diOptions = new();
            if (optionsBuilder is not null)
                optionsBuilder(diOptions);

            services.AddDbContext<OrderDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(diOptions.ConnectionStringName)));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            services.AddSingleton(configuration.GetSection(diOptions.EmailSettingsName).Get<EmailSettings>());

            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }

    /// <summary>
    /// The options for registering services.
    /// </summary>
    public class InfrastructureDiOptions
    {
        /// <summary>
        /// The name of the connection string to get from <see cref="ConfigurationExtensions.GetConnectionString"/>.
        /// </summary>
        public string ConnectionStringName { get; set; } = "OrderingDb";

        /// <summary>
        /// The section name of <see cref="EmailSettings"/> to get from <see cref="IConfiguration"/>.
        /// </summary>
        public string EmailSettingsName { get; set; } = "EmailSettings";
    }
}
