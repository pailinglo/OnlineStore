using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;




namespace OnlineStore.Models
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>
        {
            new Claim("Edit Role","true"),
            new Claim("Edit User","true")
        };


    }
}
