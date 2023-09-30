using LiteDB;
using Microsoft.Extensions.Caching.Memory;
using open_sos_id.Models;
using open_sos_id.Models.Database;

namespace open_sos_id.Services;

public interface IAuthenticationService {
	public LoginResponse? LoginUser(string username, string password);
	public void LogoutUser(string sessionId);
	public bool IsLoggedIn(string sessionId);
	public UserSessionStorageItem? GetSession(string sessionId);
	public void CreateFirstAdminUser();
}

public class AuthenticationService : IAuthenticationService {

	private readonly ILogger _logger;

	private readonly IDatabaseService _db;

	private readonly SessionStorage _sessions = new(SESSION_EXPIRES);

	public static TimeSpan SESSION_EXPIRES = TimeSpan.FromHours(6);
	public static string COOKIE_IDENTIFIER = "open-sos-id-login";

	public AuthenticationService(ILogger<DatabaseService> logger, IDatabaseService database) {
		this._logger = logger;
		this._db = database;

		this.CreateFirstAdminUser();
	}

	/// <summary>
	/// Log in a user
	/// </summary>
	/// <param name="username">Username of the user</param>
	/// <param name="password">Cleartext password of the user</param>
	/// <returns>KeyValuePair with session id as key and user as value</returns>
	public LoginResponse? LoginUser(string username, string password)
	{
		User? user = this._db.Users.Find(u => u.Name == username).FirstOrDefault();

		if (user == null) {
			return null;
		}

		if (!user.CheckPassword(password)) {
			return null;
		}

		string sessionId = this._sessions.Create(user.Id, user.IsAdmin);

		return new LoginResponse(user, sessionId);
	}

	public void LogoutUser(string sessionId) {
		this._sessions.Remove(sessionId);
	}

	public bool IsLoggedIn(string sessionId) {
		return this._sessions.Has(sessionId);
	}

	public UserSessionStorageItem? GetSession(string sessionId) {
		return this._sessions.Get(sessionId);
	}

	public void CreateFirstAdminUser() {
		if (this._db.Users.Count() > 0) return;

		User user = new User(null, "admin", "admin", true);

		this._db.Users.Insert(user);

		this._logger.LogCritical("Initial user created admin:admin");
	}
}

public class LoginResponse {
	public User User { get; }
	public string SessionId { get; }

	public LoginResponse(User user, string sessionId) {
		this.User = user;
		this.SessionId = sessionId;
	}
}


public class SessionStorage : MemoryCache
{
	private readonly TimeSpan _ttl;

	public SessionStorage(TimeSpan ttl) : base(new MemoryCacheOptions())
	{
		this._ttl = ttl;
	}

	/// <summary>
	/// Creates a new in-memory cached session for a user with a guid as session id
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="isAdmin"></param>
	/// <returns>Session Id</returns>
	public string Create(ObjectId userId, bool isAdmin) {
		string sessionId = Guid.NewGuid().ToString("N");

		this.Set<UserSessionStorageItem>(sessionId, new(userId, isAdmin), this._ttl);

		return sessionId;
	}

	public UserSessionStorageItem? Get(string sessionId) {
		return this.Get<UserSessionStorageItem>(sessionId);
	}

	public bool Has(string sessionId) {
		return this.Get(sessionId) != null;
	}
}