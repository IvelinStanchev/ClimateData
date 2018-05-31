using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeteoApp
{
    public static class RoleManager
    {
        public static bool IsUserAdmin(ClaimsPrincipal user)
        {
            return user.Identity.Name == Constants.AdminId;
        }
    }
}
