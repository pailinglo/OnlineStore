using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CategoryId { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public SelectList CategoryList { get; set; }

        public IndexModel(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
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

            CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name");
        }

        public void OnGetBrowseByCategory(int categoryId)
        {
            Products = productRepository.GetProductsByCategory(categoryId);
            CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name", categoryId);
        }
    }
}