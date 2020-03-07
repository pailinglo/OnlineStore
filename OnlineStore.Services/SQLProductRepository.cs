using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public SQLProductRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Product AddProduct(Product newProduct)
        {
            context.Add(newProduct);
            context.SaveChanges();

            return newProduct;
        }

        public Product DeleteProduct(int id)
        {
            Product toBeDeleted = context.Find<Product>(id);
            if(toBeDeleted != null)
            {
                context.Remove(toBeDeleted);
                context.SaveChanges();
            }
            return toBeDeleted;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products;
        }

        public Product GetProduct(int Id)
        {
            //??not sure why the category is not automatically populated for Product, need to figure out.
            //for now, use CategoryId
            var product = context.Products.Find(Id);
            return context.Find<Product>(Id);
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return context.Products.Where(e => e.Category.CategoryId == categoryId);
        }

        public IEnumerable<Product> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return context.Products;
            }

            return context.Products.Where(e => e.Name.Contains(searchTerm) || e.Description.Contains(searchTerm));
        }

        public Product UpdateProduct(Product updatedProduct)
        {
            //When you use the Attach method on an entity, it's state will be set to Unchanged, 
            //which will result in no database commands being generated at all. 
            //All other reachable entities with key values defined will also be set to Unchanged. 
            var product = context.Products.Attach(updatedProduct);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updatedProduct;
        }
    }
}
