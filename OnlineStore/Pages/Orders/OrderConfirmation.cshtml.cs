using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Orders
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly IOrderRepository orderRepository;

        public Order Order { get; set; }

        public OrderConfirmationModel(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public void OnGet(int orderId)
        {
            Order = orderRepository.GetOrder(orderId);
            
        }
    }
}