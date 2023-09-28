
using System.Runtime.CompilerServices;
using LiteDB;
using Microsoft.Extensions.Caching.Memory;
using open_sos_id.Models.Database;

namespace open_sos_id.Services;

public interface IAuthenticationService {
	public string? LoginUser(string username, string password);
}

public class AuthenticationService : IAuthenticationService {

	private readonly ILogger _logger;

	private readonly IDatabaseService _db;

	private readonly MemoryCache _sessions;

	public AuthenticationService(ILogger<DatabaseService> logger, IDatabaseService database) {
		this._logger = logger;
		this._db = database;
		this._sessions = new MemoryCache(new MemoryCacheOptions());
	}

	/// <summary>
	/// Log in a user
	/// </summary>
	/// <param name="username">Username of the user</param>
	/// <param name="password">Cleartext password of the user</param>
	/// <returns>Session Id or null if login failed</returns>
	public string? LoginUser(string username, string password)
	{
		User? user = this._db.GetUserCollection().Find(u => u.Name == username).FirstOrDefault();

		if (user == null) {
			return null;
		}

		if (!user.CheckPassword(password)) {
			return null;
		}

		return this.CreateSession(user.Id);
	}

	/// <summary>
	/// Creates a new in-memory cached session for a user with a guid as session id
	/// </summary>
	/// <param name="userId"></param>
	/// <returns>Session Id</returns>
	private string CreateSession(ObjectId userId) {
		string sessionId = Guid.NewGuid().ToString("N");

		this._sessions.Set<ObjectId>(sessionId, userId, TimeSpan.FromHours(2));

		return sessionId;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool ValidateSession(string sessionId) {
		return this._sessions.Get(sessionId) != null;
	}
}