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

        public Product Product { get; set; }
        public DetailsModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
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
    }
}