using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using open_sos_id.Models;
using open_sos_id.Services;

namespace open_sos_id.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationAttribute : Attribute, IAuthorizationFilter
{
	private readonly bool _isAdmin;

	public AuthorizationAttribute(bool isAdmin = false) {
		this._isAdmin = isAdmin;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		string? sessionId = context.HttpContext.Request.Cookies[AuthenticationService.COOKIE_IDENTIFIER];

		if (sessionId == null) {
			context.Result = new RedirectResult("/login");
			return;
		}

		IAuthenticationService? auth = context.HttpContext.RequestServices.GetService<IAuthenticationService>();

		if (auth == null || !auth.IsLoggedIn(sessionId)) {
			context.Result = new RedirectResult("/login");
			return;
		}

		context.HttpContext.Session = new CustomSession(sessionId);

		if (this._isAdmin) {
			UserSessionStorageItem? s = auth.GetSession(sessionId);

			if (s == null || !s.IsAdmin) {
				context.Result = new RedirectResult("/login");
				return;
			}

		} else {
			context.HttpContext.User = new ClaimsPrincipal(new UserIdentity(true));
		}
	}
}