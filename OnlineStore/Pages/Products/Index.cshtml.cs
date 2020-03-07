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
    public class IndexModel : PageModel
    {
        private readonly IProductRepository productRepository;

        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public IndexModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public void OnGet(string searchTerm)
        {
            if (searchTerm == null)
            {
                Products = productRepository.GetAllProducts();
            }
            else
            {
                Products = productRepository.Search(searchTerm);
            }

        }
    }
}