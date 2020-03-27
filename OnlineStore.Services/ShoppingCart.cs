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
            return appDbContext.Carts.Include(c=>c.Product).ToList<Cart>();

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

    }
}
