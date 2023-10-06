namespace open_sos_id.Models;

public class OAuthProviderConfiguration {
	public string ClientID = String.Empty;
	public string ClientSecret = String.Empty;
	public string RedirectURI = String.Empty;
}

public class OAuthProviders {
	public OAuthProviderConfiguration? Google;
}