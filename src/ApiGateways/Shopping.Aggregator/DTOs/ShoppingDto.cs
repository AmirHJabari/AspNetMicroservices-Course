using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.DTOs
{
    public class ShoppingDto
    {
        public ShoppingDto()
        {
            Orders = new List<OrderDto>();
        }

        public string UserName { get; set; }
        public BasketDto Basket { get; set; }
        public IEnumerable<OrderDto> Orders { get; set; }
    }
}
