using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspnetRunBasics.DTOs;

namespace AspnetRunBasics.HttpClients
{
    public interface IOrderingClient
    {
        Task<IEnumerable<OrderDto>> GetOrdersByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}
