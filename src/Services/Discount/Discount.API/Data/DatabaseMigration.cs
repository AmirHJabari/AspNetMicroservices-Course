using Dapper;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Threading;

namespace Discount.API.Data
{
    public static class DatabaseMigration
    {
        public static IServiceProvider MigratePostgreSql(this IServiceProvider services, int retry = 0)
        {
            var postgresSettings = services.GetRequiredService<IPostgresSettings>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            
            var TableName = postgresSettings.CouponTblName;

            for (int tried = 0; tried < retry; tried++)
            {
                try
                {
                    using var connection = new NpgsqlConnection(postgresSettings.ConnectionString);

                    int countTbl = connection.QueryFirst<int>(
                                            $"SELECT count(*) FROM information_schema.tables WHERE table_name = @TableName;",
                                            new { TableName });

                    if (countTbl is 0)
                    {
                        logger.LogInformation("First Init: Migrating postresql database.");

                        connection.Execute(@$"CREATE TABLE {TableName}(
                                                Id serial NOT NULL,
                                                ProductId character(24) NOT NULL,
                                                ProductName text,
                                                Description text,
                                                Amount numeric(19, 2),
                                                PRIMARY KEY (Id)
                                            );");
                        
                        logger.LogInformation($"Seeding into {TableName} table.");

                        connection.Execute(@$"INSERT INTO {TableName}(
	                            productid, productname, description, amount)
	                            VALUES ('602d2149e773f2a3990b47f5', 'IPhone X', 'Discount for Black Friday.', 351);");

                        connection.Execute(@$"INSERT INTO {TableName}(
	                            productid, productname, description, amount)
	                            VALUES ('602d2149e773f2a3990b47f6', 'Samsung 10', 'Discount for Black Friday.', 341);");
                        logger.LogInformation($"Migration was successful.");
                    }

                    break;
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "Migrating postresql database failed.");
                    Thread.Sleep(2000);
                }
            }

            return services;
        }
    }
}
