using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using LiteDB;

namespace open_sos_id.Models;

public class UserSessionStorageItem {
	public ObjectId UserId { get; set; }

	public bool IsAdmin { get; set; }

	public UserSessionStorageItem(ObjectId userId, bool isAdmin) {
		this.UserId = userId;
		this.IsAdmin = isAdmin;
	}
}

public class UserIdentity : IIdentity {

	public string? AuthenticationType { get; } = null;

	public bool IsAuthenticated { get; }

	public string? Name { get; } = null;

	public UserIdentity(bool IsAuthenticated) {
		this.IsAuthenticated = IsAuthenticated;
	}
}
public class CustomSession : ISession
{
	public bool IsAvailable { get; }

	public string Id { get; }

	public IEnumerable<string> Keys => new List<string>();

	public CustomSession(string sessionId) {
		this.IsAvailable = true;
		this.Id = sessionId;
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	public Task CommitAsync(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task LoadAsync(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public void Remove(string key)
	{
		throw new NotImplementedException();
	}

	public void Set(string key, byte[] value)
	{
		throw new NotImplementedException();
	}

	public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
	{
		throw new NotImplementedException();
	}
}