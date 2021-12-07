using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics.DTOs
{
    public class BasketDto
    {
        public BasketDto()
        {
            this.Items = new List<BasketItemDto>();
        }

        public string UserName { get; set; }
        public IList<BasketItemDto> Items { get; set; }
        public decimal BasketPrice { get; set; }
    }
}
