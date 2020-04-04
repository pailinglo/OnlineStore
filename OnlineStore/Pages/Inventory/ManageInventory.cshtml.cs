using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Services;
using OnlineStore.Models;

namespace OnlineStore.Pages.Inventory
{
    
    public class ManageInventoryModel : PageModel
    {
        private readonly IInventoryRepository inventoryRepository;
        public IEnumerable<InventoryRecord> Inventory { get; set; }

        public ManageInventoryModel(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
            Inventory = inventoryRepository.GetInventory().OrderBy(i => i.Product.Name);

        }
        public void OnGet()
        {
        
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
