using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineStore.Models;

namespace OnlineStore.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        //The validation doesn't work. an existing bug on asp.net razor page: https://github.com/dotnet/aspnetcore/issues/4895
        //[Compare("Password", ErrorMessage = "Password must match with Password Confirmation")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }
        
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Password must match with Password Confirmation";
                    return Page();
                }

                ApplicationUser user = new ApplicationUser
                {
                    Email = Email,
                    UserName = Email
                };
                
                var result = await userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //while using these Url helper, if the specified page doesn't exist, it will return null
                    var confirmationLink = Url.Page("/Account/ConfirmEmail", "OnGet", new { userId = user.Id, Token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, "Register Confirmation Link: " + confirmationLink);


                    //according to https://www.learnrazorpages.com/razor-pages/viewdata
                    //A design decision was made NOT to include a ViewBag property in the Razor Pages PageModel class

                    TempData["ErrorTitle"] = "Registration successful";
                    TempData["ErrorMessage"] = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";

                    return RedirectToPage("/Error");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }
            
            return Page();
        }
    }
}