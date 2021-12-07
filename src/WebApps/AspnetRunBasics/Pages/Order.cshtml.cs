using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.DTOs;
using AspnetRunBasics.HttpClients;

namespace AspnetRunBasics
{
    public class OrderModel : PageModel
    {
        private readonly IOrderingClient _ordering;

        public OrderModel(IOrderingClient ordering)
        {
            this._ordering = ordering;
        }

        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            Orders = await _ordering.GetOrdersByUserNameAsync("amir");

            return Page();
        }       
    }
}