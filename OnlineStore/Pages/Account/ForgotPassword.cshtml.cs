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
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ForgotPasswordModel> logger;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
            ILogger<ForgotPasswordModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string email)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(email);
                
                if(user != null && user.EmailConfirmed)
                {

                    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                    string resetPasswordUrl = Url.PageLink("ResetPassword", pageHandler: null, new { email = email, token = token }, Request.Scheme);
                    logger.LogWarning("Reset Password: ("+ System.DateTime.Now.ToLongTimeString() + "):" + resetPasswordUrl);

                    //If I only create the page as a Razor view, I will get error
                    //"No page named '/Account/ForgotPasswordConfirmation' matches the supplied values"
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
                
            }

            return Page();
        }
    }
}