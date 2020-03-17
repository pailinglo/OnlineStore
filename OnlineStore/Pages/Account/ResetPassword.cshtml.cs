using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;

namespace OnlineStore.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="New Password must be the same with Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet(string email, string token)
        {
            Email = email;
            Token = token;

        }
        
        public async Task<IActionResult> OnPost(string email, string token, string newPassword, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                //for some reason, "Compare" validator doesn't work in Razor pages
                if(newPassword != confirmPassword)
                {
                    ModelState.AddModelError(String.Empty, "New Password must be the same with Confirm Password");
                    return Page();
                }

                ApplicationUser user = await userManager.FindByEmailAsync(email);
                
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, token, newPassword);
                    if (result.Succeeded)
                    {
                        if(await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return RedirectToPage("/Account/ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                    return Page();
                }

                //Don't reveal the user not exist to avoid account enumeration and brute force attacks
                return RedirectToPage("/Account/ResetPasswordConfirmation");

            }
            return Page();
        }
    }
}