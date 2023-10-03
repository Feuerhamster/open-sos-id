using open_sos_id.Models.Database;

namespace open_sos_id.Models.ViewModels;

public class MedicalProfilesViewModel {
	public List<MedicalProfile> Profiles { get; set; } = new();

	public MedicalProfilesViewModel(List<MedicalProfile> profiles) {
		this.Profiles = profiles;
	}
}