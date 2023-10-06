using LiteDB;

namespace open_sos_id.Models.Database;

public enum BloodGroup {
	Unset, AMinus, APlus, BMinus, BPlus, ZeroMinus, ZeroPlus, ABMinus, ABPlus
}

public enum ProfileStatus {
	Active, Deactivated
}

public class MedicalProfile {
	public ObjectId Id { get; set; } = ObjectId.NewObjectId();

	[BsonRef("users")]
	public User User { get; set; }

	public string InternalName { get; set; } = String.Empty;

	public string AccessCode { get; set; } = String.Empty;

	public ProfileStatus Status { get; set; }

	public string FullName { get; set; } = String.Empty;

	public string Address { get; set; } = String.Empty;

	public Dictionary<string, string> EmergencyContacts { get; set; } = new();

	public DateTime BirthDate { get; set; }

	public int BodyWeight { get; set; }

	public BloodGroup BloodGroup { get; set; }

	public Dictionary<string, string> Doctors { get; set; } = new();

	public List<string> Allergies { get; set; } = new();

	public Dictionary<string, string> Medication { get; set; } = new();

	public List<string> PreConditions { get; set; } = new();

	public string AditionalNotes { get; set; } = String.Empty;

	public bool OrganDonorCardExists { get; set; }
}