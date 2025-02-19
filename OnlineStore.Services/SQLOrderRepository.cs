﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Services
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;

        public SQLOrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Order Create(Order newOrder)
        {
            context.Add(newOrder);
            context.SaveChanges();
            return newOrder;
        }

        public Order Delete(int id)
        {
            Order toBeDeleted = context.Find<Order>(id);
            if(toBeDeleted != null)
            {
                context.Remove(toBeDeleted);
                context.SaveChanges();
            }
            return toBeDeleted;
        }

        public IQueryable<Order> GetAllOrders()
        {
            return context.Orders;
        }

        public IQueryable<Order> GetAllOrders(string userName)
        {
            return context.Orders.Where(o => o.Username == userName);
        }

        public Order GetOrder(int orderId)
        {
            //return context.Find<Order>(new { orderId = orderId });
            var order = context.Orders.Where(o => o.OrderId == orderId).Include(o => o.OrderDetails).FirstOrDefault();
            if(order != null)
            {
                //not sure if this is the correct way???? to populate decendents.
                foreach(var orderDetail in order.OrderDetails)
                {
                    var product = context.Products.Find(orderDetail.ProductId);
                    orderDetail.Product = product;
                }
            }
            return order;
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            var orderDetails = context.OrderDetails.Where(d => d.OrderId == orderId).Include(d => d.Product);
            return orderDetails;
        }

        public IQueryable<Order> Search(string searchTerm)
        {
            return context.Orders.Where(o => o.Username.Contains(searchTerm) || o.Email.Contains(searchTerm));
        }

        public IQueryable<Order> SearchOrdersByDate(DateTime startDate, DateTime endDate)
        {
            return context.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate < endDate.AddDays(1));
        }

        public IQueryable<Order> SearchOrdersByEmail(string userEmail)
        {
            return context.Orders.Where(o => o.Email == userEmail);
        }
        public IQueryable<Order> SearchOrdersByPhone(string userPhone)
        {
            return context.Orders.Where(o => o.Phone == userPhone);
        }
        public Order Update(Order updatedOrder)
        {
            var order = context.Orders.Attach(updatedOrder);
            order.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updatedOrder;
            
        }
    }
}
