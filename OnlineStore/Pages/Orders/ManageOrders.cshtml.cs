using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Services;
using OnlineStore.Utilities;

namespace OnlineStore.Pages.Orders
{
    [Authorize(Policy = "AdminRolePolicy")]
    public class ManageOrdersModel : PageModel
    {
        
        public IOrderRepository orderRepository { get; }
        public IEnumerable<Order> Orders { get; set; }
        
        //to make custom validation attribute work, I have to put these parameters in a seperate class.
        public class InputModel
        {
            [Display(Name = "Time Span")]
            public int TimeSpan { get; set; }

            [Display(Name = "Order ID # (optional)")]
            public int? OrderId { get; set; }

            [Display(Name = "Email (optional)")]
            public string UserEmail { get; set; }

            [Display(Name = "Phone (optional)")]
            public string UserPhone { get; set; }

            [Display(Name = "Start Date")]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-2);

            [Display(Name = "End Date")]
            [DataType(DataType.Date)]
            [DateComparison("StartDate","GreaterThan", ErrorMessage = "End Date must be greater than Start Date")]
            public DateTime EndDate { get; set; } = DateTime.Today;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }


        public ManageOrdersModel(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;


        }

        public void OnGet()
        {
            Orders = orderRepository.GetAllOrders();
        }


        public IActionResult OnGetSearchOrders()
        {
            if (ModelState.IsValid)
            {
                if (Input.OrderId.HasValue && Input.OrderId > 0)
                {
                    var order = orderRepository.GetOrder(Input.OrderId.Value);
                    Orders = new List<Order>();
                    Orders = Orders.Append(order);
                }
                else if (Input.UserEmail != null)
                {
                    Orders = orderRepository.SearchOrdersByEmail(Input.UserEmail);
                }
                else if (Input.UserPhone != null)
                {
                    Orders = orderRepository.SearchOrdersByPhone(Input.UserPhone);
                }
                else if (Input.StartDate != null && Input.EndDate != null)
                {
                    Orders = orderRepository.SearchOrdersByDate(Input.StartDate, Input.EndDate);
                }

            }

            

            return Page();
        }

    }
}