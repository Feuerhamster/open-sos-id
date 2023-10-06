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
	public string InternalName { get; set; }

	[Required]
	public string FullName { get; set; }

	[Required]
	public string Address { get; set; }

	public DateTime BirthDate { get; set; }

	[Required]
	public int BodyWeight { get; set; }

	public BloodGroup BloodGroup { get; set; }
	
	public Dictionary<string, string> EmergencyContacts { get; set; } 

	public Dictionary<string, string> Doctors { get; set; }

	public List<string> Allergies { get; set; }

	public Dictionary<string, string> Medication { get; set; }

	public List<string> PreConditions { get; set; } 

	public string AditionalNotes { get; set; }

	public bool OrganDonorCardExists { get; set; }
}