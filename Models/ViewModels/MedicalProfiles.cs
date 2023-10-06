using System.ComponentModel.DataAnnotations;
using open_sos_id.Models.Database;

namespace open_sos_id.Models.ViewModels;

public class MedicalProfilesViewModel {
	public List<MedicalProfile> Profiles { get; set; } = new();

	public MedicalProfilesViewModel(List<MedicalProfile> profiles) {
		this.Profiles = profiles;
	}
}

public class MedicalProfileViewModel : ValidatableViewModel {
	[Required]
	public string InternalName { get; set; } = String.Empty;

	[Required]
	public string FullName { get; set; } = String.Empty;

	[Required]
	public string Address { get; set; } = String.Empty;

	public DateTime BirthDate { get; set; }

	[Required]
	public int BodyWeight { get; set; }

	public BloodGroup BloodGroup { get; set; }
	
	public Dictionary<string, string> EmergencyContacts { get; set; } = new();

	public Dictionary<string, string> Doctors { get; set; } = new();

	public List<string> Allergies { get; set; } = new();

	public Dictionary<string, string> Medication { get; set; } = new();

	public List<string> PreConditions { get; set; } = new();

	public string AditionalNotes { get; set; } = String.Empty;

	public bool OrganDonorCardExists { get; set; }
}