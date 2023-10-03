using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using open_sos_id.Attributes;
using open_sos_id.Models.ViewModels;
using open_sos_id.Services;

namespace open_sos_id.Controllers;

[Route("/")]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	private readonly IAuthenticationService _auth;

	public HomeController(ILogger<HomeController> logger, IAuthenticationService auth)
	{
		_logger = logger;
		_auth = auth;
	}

	public IActionResult Index() => Redirect("/login");

	[HttpGet("login")]
	public IActionResult Login() => View(new LoginViewModel());

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

	[Route("error")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
