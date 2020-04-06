using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Services;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;


namespace OnlineStore.Pages.Inventory
{
    [Authorize(Roles ="Inventory")]
    public class ManageInventoryModel : PageModel
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductRepository productRepository;

        public IEnumerable<InventoryRecord> Inventory { get; set; }
        
        public SelectList CategoryList { get; set; }
        

        public ManageInventoryModel(IInventoryRepository inventoryRepository,
            ICategoryRepository categoryRepository,
            UserManager<ApplicationUser> userManager,
            IProductRepository productRepository)
        {
            this.inventoryRepository = inventoryRepository;
            this.categoryRepository = categoryRepository;
            this.userManager = userManager;
            this.productRepository = productRepository;
            Inventory = inventoryRepository.GetInventory().OrderBy(i => i.Product.Name);
            CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name");

        }
        public void OnGet()
        {
 
        }

        public void OnGetSearchInventory(string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                ModelState.AddModelError(String.Empty, "Please enter search keyword");
            }
            else
            Inventory = inventoryRepository.SearchInventory(searchTerm).OrderBy(i => i.Product.Name); 
            
        }

        public void OnGetBrowseInventoryByCategory(int? categoryId)
        {
            
            if (categoryId.HasValue)
            {
                Inventory = inventoryRepository.SearchInventoryByCategory(categoryId.Value).OrderBy(i => i.Product.Name); ;
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Please select a category");
            }
            
        }

        public JsonResult OnPostUpdateInventory(int productId, int quantityChange)
        {
            Console.WriteLine($"Product Id = {productId} quantity change = {quantityChange}");
             
            try
            {
                int newQuantity = inventoryRepository.UpdateInventory(productId, quantityChange, userManager.GetUserName(User));
                return new JsonResult(newQuantity);
            }
            catch (Exception e)
            {
                //return new JsonResult(new { newQuantity = -1, errorMsg = e.Message });
                //this will fail the ajax request.
                throw e;
            }
            
        }


        public IActionResult OnGetViewInventoryHistory(int productId)
        {
            IEnumerable<InventoryUpdateRecord> history = inventoryRepository.GetInventoryUpdateHistory(productId);
            var product = productRepository.GetProduct(productId);
            TempData["ProductName"] = product.Name;
            TempData["ProductPhotoPath"] = product.PhotoPath;

            return Partial("_InventoryHistoryPartial", history);
            




        }


    }
}
