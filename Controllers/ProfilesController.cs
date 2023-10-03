using Microsoft.AspNetCore.Mvc;
using open_sos_id.Attributes;
using open_sos_id.Models;
using open_sos_id.Models.Database;
using open_sos_id.Models.ViewModels;
using open_sos_id.Services;

namespace open_sos_id.Controllers;

[Route("/profiles")]
[Authorization]
public class ProfilesController : Controller
{
	private readonly ILogger<HomeController> _logger;

	private readonly IAuthenticationService _auth;
	private readonly IDatabaseService _db;

	public ProfilesController(ILogger<HomeController> logger, IAuthenticationService auth, IDatabaseService db)
	{
		_logger = logger;
		_auth = auth;
		_db = db;
	}

	public IActionResult Index()
	{
		UserSessionStorageItem session = this._auth.GetSession(this.HttpContext.Session.Id);

		List<MedicalProfile> profiles = this._db.MedicalProfiles.Include(p => p.User).Find(p => p.User.Id == session.UserId).ToList();

		return View(new MedicalProfilesViewModel(profiles));
	}
}
