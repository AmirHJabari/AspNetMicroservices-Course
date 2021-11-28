using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discount.Core.Data;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Core.Extensions
{
    public static class ConfigureServices
    {
        /// <summary>
        /// Adds <see cref="IDiscountRepository"/> and <see cref="IPostgresSettings"/> to services.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="Configuration">The configuration to bind <see cref="PostgresSettings"/> from it using <paramref name="settingsSection"/>.</param>
        /// <param name="settingsSection">The section name of <see cref="PostgresSettings"/> settings.</param>
        /// <returns>The services collection.</returns>
        public static IServiceCollection AddPostgreSql(this IServiceCollection services,
            IConfiguration Configuration,
            string settingsSection = "PostgresSettings")
        {
            var postgresSettings = Configuration.GetSection(settingsSection).Get<PostgresSettings>();
            services.AddSingleton<IPostgresSettings>(postgresSettings);
            services.AddSingleton<IDiscountRepository, DiscountRepository>();

            return services;
        }
    }
}
