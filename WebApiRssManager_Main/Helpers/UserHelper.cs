using System.Security.Claims;

namespace WebApiRssManager_Main.Helpers
{
    public static class UserHelper
    {
        public static string? GetId(this ClaimsPrincipal principal) 
        {
            var userIdClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier) ?? principal.FindFirst(c => c.Type == "Id");
            if (userIdClaim != null && !string.IsNullOrWhiteSpace(userIdClaim.Value))
                return userIdClaim.Value;

            return null;
        }
    }
}
