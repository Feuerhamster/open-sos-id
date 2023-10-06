using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using open_sos_id.Attributes;
using open_sos_id.Models.ViewModels;
using open_sos_id.Services;

namespace open_sos_id.Controllers;

[Route("/")]
public class HomeController : Controller
{
	public HomeController() {}

	public IActionResult Index() => Redirect("/auth/login");

	[Route("error")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
