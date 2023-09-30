using System.ComponentModel.DataAnnotations;

namespace open_sos_id.Models.ViewModels;

public class LoginViewModel : ValidatableViewModel {
	[Required]
	[MinLength(1)]
	public string Username { get; set; } = "";

	[Required]
	[MinLength(1)]
	public string Password { get; set; } = "";
}