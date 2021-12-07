using System;
using AspnetRunBasics.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IBasketClient _basketClient;

        public IndexModel(ICatalogClient catalogClient, IBasketClient basketClient)
        {
            this._catalogClient = catalogClient;
            this._basketClient = basketClient;
        }

        public IEnumerable<CatalogDto> ProductList { get; set; } = new List<CatalogDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogClient.GetCatalogsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalogClient.GetCatalogByIdAsync(productId);
            var basket = await _basketClient.GetBasketAsync("amir");
            
            basket.Items.Add(new()
            {
                ProductId = productId,
                Price = product.Price,
                ProductName = product.Name,
                Quantity = 1,
                Color = "Black",
            });

            _ = await _basketClient.UpsertBasketAsync(basket);

            return RedirectToPage("Cart");
        }
    }
}
