using System.Security.Claims;

namespace PersonalFinanceApp.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is null)
                throw new UnauthorizedAccessException();

            return int.Parse(claim.Value);
        }
    }
}
