using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IInventoryRepository
    {
        int UpdateInventory(int productId, int quantityChange);
        IEnumerable<InventoryRecord> GetInventory();
        InventoryRecord GetInventory(int productId);
    }
}
