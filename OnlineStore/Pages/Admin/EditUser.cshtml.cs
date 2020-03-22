using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;

namespace OnlineStore.Pages.Admin
{

    public class UserCheck
    {
        //!!for [BindProperty] to work properly, get/set methods are necessary.
        //or they can't bind to the post data
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class EditUserModel : PageModel
    {
        
        
        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public List<UserCheck> UserCheckList { get; set; }

        
        [BindProperty]
        public string UserName { get; set; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EditUserModel(UserManager<ApplicationUser> userManager, 
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
            //!! have to ToList() or got error: "There is already an open DataReader associated with this Command which must be closed first."
            var roles = roleManager.Roles.ToList(); 
            foreach(var role in roles)
            {
                UserCheckList.Add(new UserCheck
                {
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name),
                    RoleId = role.Id,
                    RoleName = role.Name
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

            IList<string> existingRoles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }

            result = await userManager.AddToRolesAsync(user, UserCheckList.Where(x => x.IsSelected == true).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }

            return RedirectToPage("/Admin/ListUsers");
        }

    }


}