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
    public class ListUsersModel : PageModel
    {
        public IEnumerable<ApplicationUser> Users;
        public string SearchTerm { get; set; }

        public ListUsersModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public void OnGet()
        {

            Users = UserManager.Users.OrderBy(u=>u.Email);
        }


        public IActionResult OnGetSearchUsers(string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                Users = UserManager.Users.OrderBy(u => u.Email); ;
            }
            else
            {
                Users = UserManager.Users.Where(u => u.Email.Contains(searchTerm) || u.UserName.Contains(searchTerm)).OrderBy(u => u.Email).ToList();
            }

            return Page();
        }

    }
}