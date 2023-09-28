using LiteDB;
using open_sos_id.Models.Database;

namespace open_sos_id.Services;

public interface IDatabaseService {
	public ILiteCollection<User> GetUserCollection();
}

public class DatabaseService : IDatabaseService {

	private readonly ILogger _logger;
	private readonly IConfiguration _config;

	private LiteDatabase db;

	public DatabaseService(ILogger<DatabaseService> logger, IConfiguration config) {
		this._logger = logger;
		this._config = config;

		if (this.db != null) return;
		this.db = new LiteDatabase(this._config.GetValue<string>("Database"));

		this._logger.LogInformation("Database initialized");
	}

	public ILiteCollection<User> GetUserCollection() {
		return this.db.GetCollection<User>("users");
	}
}