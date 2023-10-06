using System.ComponentModel.DataAnnotations;

namespace open_sos_id.Models.ViewModels;

public class LoginViewModel : ValidatableViewModel {
	[Required]
	[MinLength(1)]
	public string Username { get; set; } = String.Empty;

	[Required]
	[MinLength(1)]
	public string Password { get; set; } = String.Empty;

	public OAuthManager[] OAuths { get; set; } = Array.Empty<OAuthManager>();
}