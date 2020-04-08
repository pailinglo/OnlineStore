using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Orders
{
    public class ShippingModel : PageModel
    {
        private readonly ILogger<ShippingModel> logger;
        private readonly IShoppingCart shoppingCart;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMailService mailService;
        private readonly IRazorPartialToStringRenderer razorPartialToString;

        [BindProperty]
        public OnlineStore.Models.Order Order { get; set; }
        
        public ShippingModel(ILogger<ShippingModel> logger,
            IShoppingCart shoppingCart,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IMailService mailService,
            IRazorPartialToStringRenderer razorPartialToString)
        {
            this.logger = logger;
            this.shoppingCart = shoppingCart;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.mailService = mailService;
            this.razorPartialToString = razorPartialToString;
        }
        public async Task<IActionResult> OnGet()
        {
            Order = new Order();
            ApplicationUser user = await userManager.GetUserAsync(User);
            if(user != null)
            {
                Order.Email = user.Email;
                Order.Username = user.Email;
            }

            return Page();

        }
        
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Order.OrderDate = System.DateTime.Now;

                var newOrder = orderRepository.Create(Order);
                int orderId = shoppingCart.CreateOrder(newOrder);

                await SendConfirmationEmail(Order);

                return RedirectToPage("/Orders/OrderConfirmation",new { orderId = Order.OrderId });
            }
            return Page();
        }


        public async Task SendConfirmationEmail(Order order)
        {

            try
            {
                string content = await razorPartialToString.RenderPartialToStringAsync("_OrderConfirmationEmailPartial", order);

                Message message = new Message
                {
                    From = "admin@fly.com",
                    To = new string[] { order.Email },
                    Subject = $"Order Confirmation #{order.OrderId}",
                    Body = content,
                    IsHtml = true
                };

                await mailService.Send(message);


            }
            catch(Exception e)
            {
                logger.LogWarning("Can't generate order confirmation e-mail for Order #" + order.OrderId + " because: " + e.Message);
            }

            
        }


    }
}