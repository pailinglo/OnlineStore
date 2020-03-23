using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineStore.Models;

namespace OnlineStore.Pages.Admin
{
    //[Authorize(Roles = "Admin")]
    [Authorize(Policy = "DeleteRolePolicy")]
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<ListRolesModel> logger;

        [Required]
        [Display(Name = "New Role")]
        [MaxLength(256,ErrorMessage = "Length of role name is no more than 256 characters")]
        public string RoleName { get; set; }
        public string Message { get; set; }

        public ListRolesModel(RoleManager<IdentityRole> roleManager,
            ILogger<ListRolesModel> logger)
        {
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostCreateRole(string roleName)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    RoleName = "";
                    return Page();
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        Message = "The role has been deleted successfully";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(String.Empty, error.Description);
                        }
                    }
                }
                catch(DbUpdateException ex)
                {
                    // ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY 
                    // ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;
                    // When role is deleted, the records associated with this role will be deleted. 
                    // since the foreign key constraint is ON DELETE CASCADE. need to either update the table or change code here.
                    

                    ViewData["ErrorTitle"] = $"{role.Name} role is in use";
                    ViewData["ErrorMessage"] = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";

                    logger.LogError("Update Database Error:", ex);


                    return RedirectToPage("/Error");
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "The role doesn't exist");
            }

            return Page();
        }
    }
}