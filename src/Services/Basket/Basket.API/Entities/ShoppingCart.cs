using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }
        public ShoppingCart(string username)
            : this()
        {
            this.UserName = username;
        }

        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }

        public decimal BasketPrice
        {
            get
            {
                return Items.Sum(i => i.Price * i.Quantity);
            }
        }
    }
}
