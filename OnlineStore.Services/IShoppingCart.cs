using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Services
{
    public interface IShoppingCart
    {
        public int AddToCart(int productId);
        
        //given the recordId of Cart, remove the product by 1, return the number of product left in cart
        public int RemoveFromCart(int id);

        //given the quantity and the record Id, update the cart 
        public void UpdateCart(int id, int quantity);
        public List<Cart> GetCartItems();

        //get total price of all the items in cart
        public decimal GetTotal();
        public int GetCount();
        public void EmptyCart();

    }

}
