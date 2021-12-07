using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.DTOs;
using AspnetRunBasics.HttpClients;
using System.Linq;
using System.Collections.Generic;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketClient _basket;

        public CartModel(IBasketClient basket)
        {
            _basket = basket;
        }

        public BasketDto Cart { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basket.GetBasketAsync("amir");
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var basket = await _basket.GetBasketAsync("amir");

            var itemToRemove = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            basket.Items.Remove(itemToRemove);
            
            _ = await _basket.UpsertBasketAsync(basket);

            return RedirectToPage();
        }
    }
}