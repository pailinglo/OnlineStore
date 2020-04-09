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
using OnlineStore.Services;

namespace OnlineStore.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ForgotPasswordModel> logger;
        private readonly IMailService mailService;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
            ILogger<ForgotPasswordModel> logger,
            IMailService mailService)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.mailService = mailService;
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

                    //send confirmation e-mail
                    try
                    {
                        await mailService.Send(new Message
                        {
                            To = new string[] { email },
                            Subject = "Reset Password",
                            Body = $"Please click the following link to reset your password <a href=\"{ resetPasswordUrl }\">{ resetPasswordUrl }</a>",
                            From = "admin@fly.com",
                            IsHtml = true
                        });

                    }
                    catch (Exception e)
                    {
                        logger.Log(LogLevel.Error, $"Can't send reset password link: {resetPasswordUrl} because {e.Message}");
                    }
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
                
            }

            return Page();
        }
    }
}