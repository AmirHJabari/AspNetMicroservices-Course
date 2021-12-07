using System.Collections.Generic;

namespace Shopping.Aggregator.DTOs
{
    public class BasketDto
    {
        public BasketDto()
        {
            this.Items = new List<BasketItemDto>();
        }

        public string UserName { get; set; }
        public IEnumerable<BasketItemDto> Items { get; set; }
        public decimal BasketPrice { get; set; }
    }
}
