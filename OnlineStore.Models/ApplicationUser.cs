using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
