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

	public Dictionary<EOAuthProvider, Uri> GetOAuthUrls();
}

public class AuthenticationService : IAuthenticationService {

	private readonly ILogger _logger;
	private readonly IDatabaseService _db;
	private readonly IConfiguration _config;
	private readonly SessionStorage _sessions = new(SESSION_EXPIRES);

	public static TimeSpan SESSION_EXPIRES = TimeSpan.FromHours(6);
	public static string COOKIE_IDENTIFIER = "open-sos-id-login";

	public AuthenticationService(ILogger<DatabaseService> logger, IDatabaseService database, IConfiguration config) {
		this._logger = logger;
		this._db = database;
		this._config = config;

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

	public Dictionary<EOAuthProvider, Uri> GetOAuthUrls() {
		OAuthProviders? options = this._config.GetSection("OAuth").Get<OAuthProviders>();

		Dictionary<EOAuthProvider, Uri> urls = new();

		if (options == null) return urls;

		if (options.Google != null) {
			Uri uri = new GoogleOAuthClient(options.Google).GetAuthUri();
			urls.Add(EOAuthProvider.Google, uri);
		}

		return urls;
	}

	public async Task<bool> LoginWithOAuthProvider(EOAuthProvider provider, string code) {
		List<OAuthOptions>? options = this._config.GetSection(OAuthOptions.Position).Get<List<OAuthOptions>>();

		if (options == null) return false;
		
		OAuthOptions? option = options.Find(o => o.Name == provider);

		if (option == null) return false;

		OAuthManager manager = new OAuthManager(option);

		HttpClient client = new();

		Task<HttpResponseMessage> res = client.GetAsync(manager.GetTokenUrl());

		return true;
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