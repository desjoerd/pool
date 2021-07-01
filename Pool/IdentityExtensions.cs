using System.Security.Claims;

namespace DePool
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal) 
            => principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
