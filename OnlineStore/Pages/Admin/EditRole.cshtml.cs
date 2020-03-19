using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;

namespace OnlineStore.Pages.Admin
{
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        [Required]
        [MaxLength(256, ErrorMessage = "Role Name must be within 256 characters")]        
        [BindProperty]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }

        [BindProperty]
        [Display(Name ="Role ID")]
        public string RoleId { get; set; }

        
        public SelectList UserList { get; set; }

        [BindProperty]
        public List<ApplicationUser> UsersInRole { get; set; }
        
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        public EditRoleModel(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

            UsersInRole = new List<ApplicationUser>();
            
        }
        public async Task<IActionResult> OnGet(string roleId)
        {
            await populateList(roleId);

            return Page();
        }

        private async Task populateList(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            RoleName = role.Name;
            RoleId = role.Id;


            //Got error: There is already an open DataReader associated with this Command which must be closed first.
            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, RoleName))
                {
                    UsersInRole.Add(user);
                }
            }

            //Users to be added shouldn't be in the role already:
            UserList = new SelectList(userManager.Users.Where(u => !UsersInRole.Contains(u)).ToList(), "Id", "UserName");

        }

        public async Task<IActionResult> OnPostEditRole()
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(RoleId);
                if(role == null)
                {
                    ModelState.AddModelError(String.Empty, "Can't find the role ID");
                    return Page();
                }
                role.Name = RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Admin/ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
                
                return Page();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostRemoveUserFromRole(string userId, string roleId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            IdentityRole role = await roleManager.FindByIdAsync(roleId);

            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Can't find the user");
                return Page();
            }
            if (role == null)
            {
                ModelState.AddModelError(String.Empty, "Can't find the role");
                return Page();
            }

            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                var result = await userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    await populateList(roleId);
                }
                else
                {
                    foreach(var error in result.Errors)
                    ModelState.AddModelError(String.Empty,error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "The user is not in that role");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddUserToRole(string userId, string roleId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByIdAsync(roleId);
            
            if(user == null || roleId == null)
            {
                ModelState.AddModelError(String.Empty, "Role or User is not valid");
                await populateList(roleId); 
                return Page();
            }

            var result = await userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }

            await populateList(roleId); 
            return Page();
        }
    }
}