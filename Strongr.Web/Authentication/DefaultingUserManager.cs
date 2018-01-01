using Microsoft.AspNetCore.Identity;
using Strongr.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Strongr.Web.Authentication
{
    public class DefaultingUserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DefaultingUserManager(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return this.userManager.GetUserId(principal)
                ?? "U_JOACHIM.LOVF_40GMAIL.COM";
        }
    }
}
