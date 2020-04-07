using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAllProducts();
        IQueryable<Product> Search(string searchTerm);
        Product GetProduct(int Id);
        IQueryable<Product> GetProductsByCategory(int categoryId);
        Product UpdateProduct(Product updatedProduct);
        Product AddProduct(Product newProduct);

        Product DeleteProduct(int id);
    }
}
