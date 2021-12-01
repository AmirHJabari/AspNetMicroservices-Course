using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName,
            CancellationToken cancellationToken = default)
        {
            return await this.dbContext.Orders.Where(o => o.UserName == userName)
                .ToListAsync(cancellationToken);
        }
    }
}
