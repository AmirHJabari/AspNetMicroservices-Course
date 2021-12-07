using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.DTOs;
using AspnetRunBasics.HttpClients;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogClient _catalog;
        private readonly IBasketClient _basket;

        public ProductDetailModel(ICatalogClient catalog, IBasketClient basket)
        {
            this._catalog = catalog;
            this._basket = basket;
        }

        public CatalogDto Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (productId == null)
                return NotFound();

            Product = await _catalog.GetCatalogByIdAsync(productId);

            if (Product == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalog.GetCatalogByIdAsync(productId);
            var basket = await _basket.GetBasketAsync("amir");

            basket.Items.Add(new()
            {
                ProductId = productId,
                Price = product.Price,
                ProductName = product.Name,
                Quantity = 1,
                Color = "Black",
            });

            _ = await _basket.UpsertBasketAsync(basket);

            return RedirectToPage("Cart");
        }
    }
}