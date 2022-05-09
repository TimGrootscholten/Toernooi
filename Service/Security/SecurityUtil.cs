using System.Security.Claims;

namespace Services.Security;

public static class SecurityUtil
{
    public static bool IsLoggedIn(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindAll("username").Any();
    }
    public static bool IsInScope(ClaimsPrincipal claimsPrincipal, int[] scope)
    {
        return claimsPrincipal.FindAll("scopes").Any(claim => scope.Any(x => x.ToString() == claim.Value));
    }
}