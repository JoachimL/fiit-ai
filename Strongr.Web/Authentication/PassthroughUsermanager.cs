using Microsoft.AspNetCore.Identity;
using Strongr.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Strongr.Web.Authentication
{
    public class PassthroughUsermanager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PassthroughUsermanager(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal principal)
         => userManager.GetUserId(principal);
    }
}
