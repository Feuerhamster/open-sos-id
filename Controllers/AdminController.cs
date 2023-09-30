using Microsoft.AspNetCore.Mvc;
using open_sos_id.Attributes;
using open_sos_id.Services;

namespace open_sos_id.Controllers;

[Route("/admin")]
[Authorization(isAdmin: true)]
public class AdminController : Controller
{
	private readonly ILogger<HomeController> _logger;

	private readonly IAuthenticationService _auth;

	public AdminController(ILogger<HomeController> logger, IAuthenticationService auth)
	{
		_logger = logger;
		_auth = auth;
	}

	public IActionResult Index()
	{
		return View();
	}
}
