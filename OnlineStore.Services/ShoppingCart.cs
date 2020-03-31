using Microsoft.AspNetCore.Http;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Services
{
    public class ShoppingCart : IShoppingCart
    {
        public string ShoppingCardId { get; set; }
        public static string CartSessionId = "CartSessionId";
        private readonly AppDbContext appDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCart(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.ShoppingCardId = getCardId(httpContextAccessor.HttpContext);
        }
        
        private static string getCardId(HttpContext context)
        {
            if(context.Session.GetString(CartSessionId) == null)
            {
                if (String.IsNullOrWhiteSpace(context.User.Identity.Name))
                    context.Session.SetString(CartSessionId, Guid.NewGuid().ToString());
                else
                    context.Session.SetString(CartSessionId, context.User.Identity.Name);
            }

            return context.Session.GetString(CartSessionId);
        }


        public int AddToCart(int productId)
        {
            var cart = appDbContext.Carts.FirstOrDefault<Cart>(e => e.CartId == ShoppingCardId && e.ProductId == productId);
            if(cart == null)
            {
                cart = new Cart
                {
                    CartId = ShoppingCardId,
                    ProductId = productId,
                    Count = 1,
                    DateCreated = System.DateTime.Now
                };
                appDbContext.Carts.Add(cart);
            }
            else
            {
                cart.Count++;
                
                
            }
            //??will this reflect the change? if not, check SQLProductRepository.
            appDbContext.SaveChanges();
            return cart.Count;
        }
        //given the recordId of Cart, remove the product by 1, return the number of product left in cart
        public int RemoveFromCart(int id)
        {
            var cart = appDbContext.Carts.FirstOrDefault<Cart>(e => e.RecordId == id);
            if (cart != null)
            {
                cart.Count--;
                if(cart.Count== 0)
                {
                    appDbContext.Carts.Remove(cart);
                }
                appDbContext.SaveChanges();
                return cart.Count;
            }
            return 0;
        }

        //given the quantity and the record Id, update the cart 
        public void UpdateCart(int id, int quantity)
        {
            var cart = appDbContext.Carts.FirstOrDefault<Cart>(e => e.RecordId == id);
            if (cart != null)
            {
                cart.Count = quantity;
                if (cart.Count == 0)
                {
                    appDbContext.Carts.Remove(cart);
                }
                appDbContext.SaveChanges();
            }
        }

        public List<Cart> GetCartItems()
        {
            //return appDbContext.Carts.ToList<Cart>();
            //"Include" - specify related entity to be included in the query result.
            //Here we need to include Product as well, or the Product would be null.
            //using Microsoft.EntityFrameworkCore;
            return appDbContext.Carts.Where<Cart>(e => e.CartId == ShoppingCardId)
                .Include(c=>c.Product).ToList<Cart>();

        }
        //get total price of all the items in cart
        public decimal GetTotal()
        {
            return appDbContext.Carts.Where<Cart>(e => e.CartId == ShoppingCardId).Sum(c => c.Count * c.Product.Price);
        }
        //get total count of all the items in cart
        public int GetCount()
        {
            return appDbContext.Carts.Where<Cart>(e => e.CartId == ShoppingCardId).Sum(c => c.Count);
        }

        public void EmptyCart()
        {
            var carts = appDbContext.Carts.Where<Cart>(e => e.CartId == ShoppingCardId);
            appDbContext.Carts.RemoveRange(carts);
            appDbContext.SaveChanges();
        }

        //converts the shopping cart to an order during the checkout phase.
        //return the order Id as confirmed number
        public int CreateOrder(Order order)
        {
            var carItems = GetCartItems();
            decimal orderTotal = 0;
            foreach(var item in carItems)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = item.Product.Price
                };
                appDbContext.Add<OrderDetail>(detail);
                orderTotal += item.Count * item.Product.Price;
            }
            order.Total = orderTotal;
            var orderTracking = appDbContext.Attach(order);
            orderTracking.State = EntityState.Modified;

            appDbContext.SaveChanges();

            EmptyCart();

            return order.OrderId;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = appDbContext.Carts.Where<Cart>(e => e.CartId == ShoppingCardId);
            foreach(Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            appDbContext.SaveChanges();
            //change the cartId saved in session to the login user.
            httpContextAccessor.HttpContext.Session.SetString(CartSessionId, userName);
        }
    }
}
