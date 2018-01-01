using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Strongr.Web.Authentication
{
    public interface IUserManager
    {
        string GetUserId(ClaimsPrincipal principal);
    }
}
