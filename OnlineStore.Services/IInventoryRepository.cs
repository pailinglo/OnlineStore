using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IInventoryRepository
    {
        int UpdateInventory(int productId, int quantityChange, string updateBy);
        IEnumerable<InventoryRecord> GetInventory();
        InventoryRecord GetInventory(int productId);

        IEnumerable<InventoryRecord> SearchInventory(string searchTerm);

        IEnumerable<InventoryRecord> SearchInventoryByCategory(int categoryId);

        IEnumerable<InventoryUpdateRecord> GetInventoryUpdateHistory(int productId);

    }
}
