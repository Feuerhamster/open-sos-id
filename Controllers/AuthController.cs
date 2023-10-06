using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using open_sos_id.Attributes;
using open_sos_id.Models;
using open_sos_id.Models.ViewModels;
using open_sos_id.Services;

namespace open_sos_id.Controllers;

[Route("/auth")]
public class AuthController : Controller
{
	private readonly IAuthenticationService _auth;

	public AuthController(IAuthenticationService auth)
	{
		_auth = auth;
	}

	public IActionResult Index() => Redirect("/login");

	[HttpGet("login")]
	public IActionResult Login() {
		OAuthManager[] managers = this._auth.GetOAuthManagers();
		
		return View(new LoginViewModel() { OAuths = managers });
	}

	[HttpPost("login")]
	public IActionResult Login(LoginViewModel data)
	{
		if (!ModelState.IsValid) return View(new LoginViewModel() { IsValid = false });

		LoginResponse? login = this._auth.LoginUser(data.Username, data.Password);

		if (login == null) return View(new LoginViewModel() { IsValid = false });

		CookieOptions cookieOptions = new() {
			HttpOnly = true,
			SameSite = SameSiteMode.Strict
		};

		this.Response.Cookies.Append(AuthenticationService.COOKIE_IDENTIFIER, login.SessionId, cookieOptions);

		if (login.User.IsAdmin) {
			return Redirect("/admin");
		} else {
			return Redirect("/profiles");
		}
	}

	[HttpGet("logout")]
	[Authorization(isAdmin: false)]
	public IActionResult Logout()
	{
		this._auth.LogoutUser(this.HttpContext.Session.Id);

		this.Response.Cookies.Delete(AuthenticationService.COOKIE_IDENTIFIER);

		return Redirect("/login");
	}

	[HttpGet("oauth/{provider}")]
	public IActionResult OAuth(string provider, string code) {
		return View();
	}
}
