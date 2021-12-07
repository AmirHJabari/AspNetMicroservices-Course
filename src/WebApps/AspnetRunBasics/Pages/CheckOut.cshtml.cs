using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.DTOs;
using AspnetRunBasics.HttpClients;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketClient _basket;
        private readonly IOrderingClient _ordering;

        public CheckOutModel(IBasketClient basket, IOrderingClient ordering)
        {
            this._basket = basket;
            this._ordering = ordering;
        }

        [BindProperty]
        public BasketCheckoutDto Order { get; set; }

        public BasketDto Cart { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basket.GetBasketAsync("amir");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _basket.GetBasketAsync("amir");

            if (!ModelState.IsValid)
                return Page();

            Order.UserName = "amir";

            await _basket.CheckoutBasketAsync(Order);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}