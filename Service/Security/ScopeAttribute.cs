using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;

namespace Services.Security;

public class ScopeAttribute : TypeFilterAttribute
{
    public ScopeAttribute(params TournamentPermissions[] scopes) : base(typeof(ScopeFilter))
    {
        Arguments = new object[] {scopes};
    }

    private class ScopeFilter : IAuthorizationFilter
    {
        private readonly int[] _scopes;

        public ScopeFilter(params TournamentPermissions[] scopes)
        {
            _scopes = Array.ConvertAll(scopes, x => (int) x);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If not authorized at all, don't bother
            if (context.Result != null) return;
            var claimsIdentity = context.HttpContext.User;
            if (claimsIdentity == null) return;

            var isLoggedIn = SecurityUtil.IsLoggedIn(claimsIdentity);
            var isInScope = SecurityUtil.IsInScope(claimsIdentity, _scopes);
            if (!isLoggedIn || isLoggedIn && !isInScope && _scopes.Length > 0)
            {
                HandleForbiddenRequest(context);
            }
        }

        private static void HandleForbiddenRequest(AuthorizationFilterContext context)
        {
            if (context == null) throw new ArgumentException(null, nameof(context));
            context.Result = new ForbidResult();
        }
    }
}