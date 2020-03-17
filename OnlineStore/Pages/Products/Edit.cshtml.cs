using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Services;
using OnlineStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public Product Product { get; set; }
        
        public SelectList CategoryList { get; set; }
        
        [BindProperty]
        public IFormFile Photo { get; set; }

        public EditModel(IProductRepository productRepository, 
                         ICategoryRepository categoryRepository,
                         IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        
        }
        
        public IActionResult OnGet(int? productId)
        {
            
            if (productId.HasValue)
            {
                Product = productRepository.GetProduct(productId.Value);                
            }
            else
            {
                Product = new Product();
            }

            if (Product == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (Product.CategoryId > 0)
                CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name", Product.CategoryId);
            else
                CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name");


            return Page();
        }

        public IActionResult OnPost()
        {
            
            if (ModelState.IsValid)
            {
                //if user upload a file
                if (Photo != null)
                {
                    //delete the existing photo of the employee first
                    if (Product.PhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                            "images", Product.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object
                    Product.PhotoPath = ProcessUploadedFile();
                }

                

                if (Product.ProductId > 0)
                {
                    this.productRepository.UpdateProduct(Product);
                }
                else
                {
                    this.productRepository.AddProduct(Product);
                }

                return RedirectToPage("Index");
            }

            CategoryList = new SelectList(categoryRepository.GetAllCategories(), "CategoryId", "Name", Product.CategoryId);

            return Page();
        
        }

        //This method helps to generate an unique file name for the uploaded file
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                //use webHostEnvironment to get the root folder wwwrroot
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

                //append GUID to the uploaded file name to make it unique and avoid users upload files with same file name
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Photo.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


    }
}