namespace open_sos_id.Models;

public class LoginViewModel : ValidatableViewModel {
	public string? Username { get; set; }

	public string? Password { get; set; }
}