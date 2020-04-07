using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using OnlineStore.Services;
using OnlineStore.Utilities;

namespace OnlineStore.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IShoppingCart shoppingCart;

        [BindProperty(SupportsGet =true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        public PaginatedList<Product> Products { get; set; }
        public SelectList CategoryList { get; set; }
        public int PageSize { get; set; }
        public int? PageIndex { get; set; }

        public IndexModel(IProductRepository productRepository, 
            ICategoryRepository categoryRepository,
            IShoppingCart shoppingCart)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.shoppingCart = shoppingCart;
            PageSize = 3;
        }
        public async Task<IActionResult> OnGet(string searchTerm, int? categoryId, int? pageIndex)
        {
            if (searchTerm == null && !categoryId.HasValue)
            {
                Products = await PaginatedList<Product>.CreateAsync(productRepository.GetAllProducts(), pageIndex??1, PageSize);
            }
            else if (searchTerm != null)
            {
                Products = await PaginatedList<Product>.CreateAsync(productRepository.Search(searchTerm), pageIndex??1, PageSize);

            }
            else if (categoryId.HasValue)
            {
                Products = await PaginatedList<Product>.CreateAsync(productRepository.GetProductsByCategory(categoryId.Value), pageIndex ?? 1, PageSize);
            }

            CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name");
            return Page();
        }

        
        //add to cart.
        public IActionResult OnPostAddToCart(int productId)
        {
            int totalCount = shoppingCart.AddToCart(productId);
            return RedirectToPage("/Orders/ViewShoppingCart");
        }
    }
}