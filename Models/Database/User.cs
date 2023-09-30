using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using Isopoh.Cryptography.Argon2;
using LiteDB;

namespace open_sos_id.Models.Database;

public enum EOAuthProvider {
	Google
}

public class User {
	public ObjectId Id { get; private set; } = ObjectId.NewObjectId();

	public string? EMail { get; set; }

	public string Name { get; set; }

	public DateTime DateCreated { get; set; } = DateTime.Now;

	public string? Password { get; private set; }

	public string? PasswordSalt { get; private set; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

	public bool IsAdmin { get; set; }

	public Dictionary<EOAuthProvider, string> OAuth { get; private set; } = new Dictionary<EOAuthProvider, string>();


	public User(string? email, string name, string password, bool isAdmin) {
		this.EMail = email;
		this.Name = name;
		this.Password = Argon2.Hash(password, this.PasswordSalt);
		this.IsAdmin = isAdmin;
	}

	[BsonCtor]
	public User(ObjectId _id, string eMail, string name, DateTime dateCreated, string? password, string? passwordSalt, bool isAdmin, Dictionary<EOAuthProvider, string> oAuth) {
		this.Id = _id;
		this.EMail = eMail;
		this.Name = name;
		this.DateCreated = dateCreated;
		this.Password = password;
		this.PasswordSalt = passwordSalt;
		this.IsAdmin = isAdmin;
		this.OAuth = oAuth;
	}

	public bool CheckPassword(string password) {
		return Argon2.Verify(this.Password, password, this.PasswordSalt);
	}

	public void SetPassword(string password) {
		this.Password = Argon2.Hash(password, this.PasswordSalt);
	}
}