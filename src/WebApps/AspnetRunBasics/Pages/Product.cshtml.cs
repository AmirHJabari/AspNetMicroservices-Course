using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.DTOs;
using AspnetRunBasics.HttpClients;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogClient _catalog;
        private readonly IBasketClient _basket;

        public ProductModel(ICatalogClient catalog, IBasketClient basket)
        {
            this._catalog = catalog;
            this._basket = basket;
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogDto> ProductList { get; set; } = new List<CatalogDto>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string category)
        {
            var productList = await _catalog.GetCatalogsAsync();
            CategoryList = productList.Select(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(category))
            {
                ProductList = productList.Where(p => p.Category.Contains(category, StringComparison.OrdinalIgnoreCase));
                SelectedCategory = category;
            }
            else
            {
                ProductList = productList;
            }

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
