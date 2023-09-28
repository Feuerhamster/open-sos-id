using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using open_sos_id.Models;

namespace open_sos_id.Controllers;

[Route("/")]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return Redirect("/login");
	}

	[Route("privacy")]
	public IActionResult Privacy()
	{
		return View();
	}

	[HttpGet("login")]
	public IActionResult Login()
	{
		return View(new LoginViewModel());
	}

	[HttpPost("login")]
	public IActionResult Login(LoginViewModel data)
	{
		return View(new LoginViewModel() { IsValid = false });
	}

	[Route("error")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
