namespace open_sos_id.Models.ViewModels;

public class ValidatableViewModel {
	public bool? IsValid { get; set; }

	public string? Invalid() {
		if (this.IsValid == null) return null;
		if (this.IsValid == false) {
			return "true";
		} else {
			return "false";
		}
	}
}