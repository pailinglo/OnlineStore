using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;

namespace OnlineStore.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToPage("/Products/Index");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"The User Id {userId} is invalid";
                return RedirectToPage("NotFound");
            }
            //validate if an email confirmation token matches the specified user.
            //which flips the flag in dbo.AspNetUsers table "EmailConfirmed" to be true if matched.
            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Page();
            }
            //if not succeeded:
            TempData["ErrorTitle"] = "Error";
            TempData["ErrorMessage"] = "E-mail can't be confirmed";
            return RedirectToPage("Error");
        }
    }
}