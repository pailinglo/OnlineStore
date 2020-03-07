using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> Search(string searchTerm);
        Product GetProduct(int Id);
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        Product UpdateProduct(Product updatedProduct);
        Product AddProduct(Product newProduct);

        Product DeleteProduct(int id);
    }
}
