using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;

namespace OnlineStore.Pages.Admin
{
    
    public class EditUserClaimsModel : PageModel
    {
        public class UserCheck
        {
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
            public bool IsSelected { get; set; }
        }

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public List<UserCheck> UserCheckList { get; set; }


        [BindProperty]
        public string UserName { get; set; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EditUserClaimsModel(UserManager<ApplicationUser> userManager,            
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }

            UserId = user.Id;
            UserName = user.UserName;
            UserCheckList = new List<UserCheck>();
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            foreach (var claim in ClaimStore.AllClaims)
            {

                UserCheckList.Add(new UserCheck
                {
                    //somehow User.HasClaim doesn't work properly? some claim doesn't included.
                    //IsSelected = User.HasClaim(c => c.Type == claim.Type && c.Value == claim.Value),
                    IsSelected = existingUserClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value),
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
           
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            ApplicationUser user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }

            IList<Claim> existingClaims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, existingClaims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return Page();
            }

            result = await userManager.AddClaimsAsync(user, UserCheckList.Where(r => r.IsSelected == true).Select(c => new Claim(c.ClaimType, c.ClaimValue)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return Page();
            }

            return RedirectToPage("/Admin/ListUsers");
        }

    }

}