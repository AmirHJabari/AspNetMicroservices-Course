using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class SeedOrderContext
    {
        public static async Task SeedAsync(OrderDbContext context, ILogger<SeedOrderContext> logger)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.AddRangeAsync(GetPreconfiguredOrders());
                await context.SaveChangesAsync();

                logger.LogInformation($"Seeding data with {nameof(OrderDbContext)} to databse was successful.");
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            yield return new()
            {
                UserName = "amir",
                Country = "Iran",
                EmailAddress = "amir@email.com",
                FirstName = "Amir H.",
                LastName = "Jabari",
                State = "Kermanshah",
                TotalPrice = 350,
                ZipCode = "fs544f"
            };
        }
    }
}
