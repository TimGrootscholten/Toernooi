using System.Security.Claims;
using Models;

namespace Services.Security;

public static class SecurityUtil
{
    public static bool IsInScope(ClaimsPrincipal claimsPrincipal, int[] scope)
    {
        return claimsPrincipal.FindAll("scopes").Any(claim => scope.Any(x => x.ToString() == claim.Value));
    }
}