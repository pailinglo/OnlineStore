using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Services;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Pages.Inventory
{
    [Authorize(Roles ="Inventory")]
    public class ManageInventoryModel : PageModel
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly ICategoryRepository categoryRepository;

        public IEnumerable<InventoryRecord> Inventory { get; set; }
        
        public SelectList CategoryList { get; set; }
        
        

        public ManageInventoryModel(IInventoryRepository inventoryRepository,
            ICategoryRepository categoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
            this.categoryRepository = categoryRepository;
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
                int newQuantity = inventoryRepository.UpdateInventory(productId, quantityChange);
                return new JsonResult(newQuantity);
            }
            catch (Exception e)
            {
                //return new JsonResult(new { newQuantity = -1, errorMsg = e.Message });
                //this will fail the ajax request.
                throw e;
            }
            
        }

    }
}
