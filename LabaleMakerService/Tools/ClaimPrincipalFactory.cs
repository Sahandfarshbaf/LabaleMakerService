using System.Linq;
using System.Security.Claims;

namespace CarDeal.Tools
{
    public static class ClaimPrincipalFactory
    {
        public static long GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "Id").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault() ?? string.Empty);
        }
        public static long GetFullName(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "FullName").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault() ?? string.Empty);
        }

        public static long GetRoalId(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "roleId").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault() ?? string.Empty);
        }
        public static long GetUserRoleId(ClaimsPrincipal claimsPrincipal)
        {
            return long.Parse(claimsPrincipal.Claims.Where(c => c.Type == "UserRoleId").Select(x => x.Value).DefaultIfEmpty("0").FirstOrDefault() ?? string.Empty);
        }
    }
}
