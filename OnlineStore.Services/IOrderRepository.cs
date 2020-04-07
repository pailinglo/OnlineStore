using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IOrderRepository
    {
        Order Create(Order newOrder);
        Order Delete(int id);
        Order Update(Order updatedOrder);
        IQueryable<Order> GetAllOrders();
        IQueryable<Order> GetAllOrders(string userName);
        IQueryable<Order> Search(string searchTerm);
        IEnumerable<OrderDetail> GetOrderDetails(int orderId);
        Order GetOrder(int orderId);
        IQueryable<Order> SearchOrdersByDate(DateTime startDate, DateTime endDate);
        IQueryable<Order> SearchOrdersByEmail(string userEmail);
        IQueryable<Order> SearchOrdersByPhone(string userPhone);
    }
}
