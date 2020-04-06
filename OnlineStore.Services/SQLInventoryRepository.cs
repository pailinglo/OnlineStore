using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Services
{
    public class SQLInventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext appDbContext;

        public SQLInventoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IEnumerable<InventoryRecord> GetInventory()
        {
            return appDbContext.Inventory.Include(i=>i.Product);
        }

        public InventoryRecord GetInventory(int productId)
        {
            return appDbContext.Inventory.Where(i => i.ProductId == productId).FirstOrDefault();
        }

        public IEnumerable<InventoryUpdateRecord> GetInventoryUpdateHistory(int productId)
        {
            return appDbContext.InventoryHistory.Where(i => i.ProductId == productId);
        }

        public IEnumerable<InventoryRecord> SearchInventory(string searchTerm)
        {
            return appDbContext.Inventory.Include(i => i.Product).Where(i => i.Product.Name.Contains(searchTerm));
        }

        public IEnumerable<InventoryRecord> SearchInventoryByCategory(int categoryId)
        {
            return appDbContext.Inventory.Include(i => i.Product).Where(i => i.Product.CategoryId == categoryId);

        }

        public int UpdateInventory(int productId, int quantityChange, string updateBy)
        {
            var record = appDbContext.Inventory.Where(r => r.ProductId == productId).FirstOrDefault();
            if(record != null)
            {
                record.QuantityOnHand += quantityChange;
                record.UpdateTime = System.DateTime.Now;

                if(record.QuantityOnHand < 0)
                {
                    throw new Exception("Inventory quantity can't be less than zero.");
                }

                var history = new InventoryUpdateRecord
                {
                    ProductId = productId,
                    QuantityChange = quantityChange,
                    UpdateTime = System.DateTime.Now,
                    UpdateBy = updateBy

                };

                appDbContext.Attach(record);
                appDbContext.Add(history);
                appDbContext.SaveChanges();

                return record.QuantityOnHand;
            }
            return 0;
        }
    }
}
