using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    interface ICategoryRepository
    {

        IEnumerable<Category> GetAllCategories();
        IEnumerable<Product> Search(string searchTerm);
        Category GetCategory(int Id);
        Product UpdateCategory(Product updatedCategory);
        Product AddCategory(Product newCategory);

        Product DeleteCategory(int id);
    }
}
