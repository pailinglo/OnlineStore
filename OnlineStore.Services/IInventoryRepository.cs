using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IInventoryRepository
    {
        int UpdateInventory(int productId, int quantityChange, string updateBy);
        IQueryable<InventoryRecord> GetInventory();
        InventoryRecord GetInventory(int productId);

        IQueryable<InventoryRecord> SearchInventory(string searchTerm);

        IQueryable<InventoryRecord> SearchInventoryByCategory(int categoryId);

        IQueryable<InventoryUpdateRecord> GetInventoryUpdateHistory(int productId);

    }
}
