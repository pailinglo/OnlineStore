using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly IShoppingCart shoppingCart;

        public Product Product { get; set; }
        public DetailsModel(IProductRepository productRepository,
            IShoppingCart shoppingCart)
        {
            this.productRepository = productRepository;
            this.shoppingCart = shoppingCart;
        }
        public IActionResult OnGet(string productId)
        {
            Product = productRepository.GetProduct(Convert.ToInt32(productId));
            if(Product == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int productId)
        {
            int totalCount = shoppingCart.AddToCart(productId);
            return RedirectToPage("/Order/ViewShoppingCart");
        }

    }
}