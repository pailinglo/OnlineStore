using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IShoppingCart shoppingCart;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [BindProperty]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public LoginModel(UserManager<ApplicationUser> userManager, 
                          SignInManager<ApplicationUser> signInManager,
                          IShoppingCart shoppingCart)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.shoppingCart = shoppingCart;
        }
        public void OnGet()
        {
        }

        //also handles the return url
        public async Task<IActionResult> OnPost(string email, string password, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                //first, see if the e-mail is confirmed 
                ApplicationUser user = await userManager.FindByEmailAsync(email);
                if (user != null && !user.EmailConfirmed && await userManager.CheckPasswordAsync(user,password))
                {
                    ModelState.AddModelError(String.Empty, "Please confirm your e-mail");
                    return Page();
                }

                //then sign user in 
                var result = await signInManager.PasswordSignInAsync(email, password, RememberMe, true);
                if (result.Succeeded)
                {
                    //to prevent open redirect attack, check if Url is local before redirect. or use LocalRedirect.
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        //return RedirectToPage(returnUrl);
                        //return RedirectToPage("/Products/Edit/2");
                        //if using above method, I will get error "InvalidOperationException: No page named '/Products/Edit/2' matches the supplied values."

                        //in case there is a shopping cart, migrate the items in shopping cart to user's name
                        shoppingCart.MigrateCart(user.UserName);

                        return Redirect(returnUrl);
                    }

                    //in case there is a shopping cart, migrate the items in shopping cart to user's name
                    shoppingCart.MigrateCart(user.UserName);
                    return RedirectToPage("/Products/Index");
                }

                //if the user has been locked out.
                if (result.IsLockedOut)
                {
                    TempData["ErrorTitle"] = "Account locked out";
                    TempData["ErrorMessage"] = "You have been locked out. Please wait for some time or reset your password";

                    //create a account locked out page later and redirect to this page.
                    return RedirectToPage("/Error");
                }

                ModelState.AddModelError(String.Empty, "Failed login attempt");

            }


            return Page();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Products/Index");
        }
    }
}
