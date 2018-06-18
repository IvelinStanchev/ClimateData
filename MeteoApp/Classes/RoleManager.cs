using System.Security.Claims;
using MeteoApp.Common;

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
