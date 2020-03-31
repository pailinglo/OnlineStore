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

        [BindProperty]
        public OnlineStore.Models.Order Order { get; set; }
        
        public ShippingModel(ILogger<ShippingModel> logger,
            IShoppingCart shoppingCart,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.shoppingCart = shoppingCart;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            
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
        
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Order.OrderDate = System.DateTime.Now;

                var newOrder = orderRepository.Create(Order);
                int orderId = shoppingCart.CreateOrder(newOrder);

                logger.LogInformation("Test Order:" + Order.Address);

                return RedirectToPage("/Orders/OrderConfirmation",new { orderId = Order.OrderId });
            }
            return Page();
        }

    }
}