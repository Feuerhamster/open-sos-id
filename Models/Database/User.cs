using Isopoh.Cryptography.Argon2;
using LiteDB;
using System.Security.Cryptography;

namespace open_sos_id.Models.Database;

public enum EOAuthProvider {
	Google
}

public class User {
	public ObjectId Id { get; private set; }

	public string? EMail { get; set; }

	public string Name { get; set; }

	public DateTime DateCreated { get; private set; }

	private string Password { get; set; }

	private string PasswordSalt { get; set; }

	public bool IsAdmin { get; set; }

	[BsonField("oauth")]
	public Dictionary<EOAuthProvider, string> OAuth { get; private set; }

	public User(string email, string name, string password, bool admin) {
		this.Id = ObjectId.NewObjectId();
		this.EMail = email;
		this.Name = name;
		this.DateCreated = DateTime.Now;
		this.Password = password;
		this.PasswordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
		this.IsAdmin = admin;
		this.OAuth = new Dictionary<EOAuthProvider, string>();
	}

	[BsonCtor]
	public User(ObjectId _id, string email, string name, DateTime dateCreated, string password, string passwordSalt, bool admin, Dictionary<EOAuthProvider, string> oauth) {
		this.Id = _id;
		this.EMail = email;
		this.Name = name;
		this.DateCreated = dateCreated;
		this.Password = password;
		this.PasswordSalt = passwordSalt;
		this.IsAdmin = admin;
		this.OAuth = oauth;
	}

	public bool CheckPassword(string password) {
		return Argon2.Verify(this.Password, password, this.PasswordSalt);
	}

	public void SetPassword(string password) {
		this.Password = Argon2.Hash(password, this.PasswordSalt);
	}
}