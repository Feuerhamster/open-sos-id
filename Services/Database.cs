using LiteDB;
using open_sos_id.Models.Database;

namespace open_sos_id.Services;

public interface IDatabaseService {
	public ILiteCollection<User> Users { get; }
}

public class DatabaseService : IDatabaseService {

	private readonly ILogger _logger;
	private readonly IConfiguration _config;

	private readonly LiteDatabase _db;

	public ILiteCollection<User> Users { get; private set; }

	public DatabaseService(ILogger<DatabaseService> logger, IConfiguration config) {
		this._logger = logger;
		this._config = config;

		this._db = new LiteDatabase(this._config.GetValue<string>("Database"));

		this.Users = this._db.GetCollection<User>("users");

		this._logger.LogInformation("Database initialized");
	}
}