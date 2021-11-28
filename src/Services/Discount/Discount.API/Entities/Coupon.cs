using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Entities
{
    public class Coupon
    {
        public int Id { get; set; }

        [StringLength(24, MinimumLength = 24)]
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
