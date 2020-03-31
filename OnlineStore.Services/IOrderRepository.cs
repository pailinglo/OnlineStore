using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IOrderRepository
    {
        Order Create(Order newOrder);
        Order Delete(int id);
        Order Update(Order updatedOrder);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetAllOrders(string userName);
        IEnumerable<Order> Search(string searchTerm);
        IEnumerable<OrderDetail> GetOrderDetails(int orderId);
        Order GetOrder(int orderId);
    }
}
