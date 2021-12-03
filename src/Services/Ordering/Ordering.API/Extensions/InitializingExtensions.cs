using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Ordering.API.Extensions
{
    public static class InitializingExtensions
    {
        public static IServiceProvider MigrateDatabase<TDbContext>(this IServiceProvider services,
                                        Action<TDbContext, IServiceProvider> seeder,
                                        int retry = 0)
            where TDbContext : DbContext
        {
            var logger = services.GetRequiredService<ILogger<TDbContext>>();
            var dbContext = services.GetRequiredService<TDbContext>();

            for (int tried = 0; tried < retry; tried++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    seeder(dbContext, services);

                    break;
                }
                catch (SqlException ex) when (tried < retry - 1) // don't catch in the last try
                {
                    logger.LogError(ex, "Migrating database failed.");
                    Thread.Sleep(2000);
                }
            }

            return services;
        }

        public static bool IsDocker(this IWebHostEnvironment env) => env.IsEnvironment("Docker");

    }
}
