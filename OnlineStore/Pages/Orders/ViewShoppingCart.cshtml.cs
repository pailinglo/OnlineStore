using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Orders
{
    public class ViewShoppingCartModel : PageModel
    {
        private readonly IShoppingCart shoppingCart;
        
        public List<Cart> CartItems { get; set; }
        public decimal Total { get; set; }
        public int TotalCount { get; set; }
        public ViewShoppingCartModel(IShoppingCart shoppingCart)
        {
            this.shoppingCart = shoppingCart;
        }
        public void OnGet()
        {
            CartItems = shoppingCart.GetCartItems();
            TotalCount = CartItems.Sum(c => c.Count);
            Total = CartItems.Sum(c => c.Count * c.Product.Price);
        }

        public IActionResult OnPostUpdateShoppingCart(int recordId, int count)
        {
            shoppingCart.UpdateCart(recordId, count);
            CartItems = shoppingCart.GetCartItems();
            TotalCount = CartItems.Sum(c => c.Count);
            Total = CartItems.Sum(c => c.Count * c.Product.Price);

            return Page();
        }


        public IActionResult OnGetProceedToCheckout()
        {
            //check if user logged in, if not, redirect to login page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Orders/Shipping");
            }
            else
            {
                return RedirectToPage("/Account/Login", new { returnUrl = "/Orders/Shipping" });
            }

        }


    }
}
