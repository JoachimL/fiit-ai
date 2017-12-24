using Microsoft.AspNetCore.Identity;
using Strongr.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Strongr.Web
{
    public static class UserManagerExtensions
    {
        public static async Task<string> GetUserId(this UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            var userId = user.Id;
            return userId;
        }

    }
}
