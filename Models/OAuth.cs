namespace open_sos_id.Models;


public class OAuthClient {

	private readonly string AuthUri;
	private readonly string TokenUri;
	private readonly string Scopes;
	private readonly OAuthProviderConfiguration Configuration;

	public OAuthClient(OAuthProviderConfiguration config, string authUri, string tokenUri, string scopes) {
		this.Configuration = config;	
		this.AuthUri = authUri;
		this.TokenUri = tokenUri;
		this.Scopes = scopes;
	}

	public Uri GetAuthUri()
	{
		UriBuilder uri = new(this.AuthUri);

		uri.Query = new QueryString()
			.Add("client_id", this.Configuration.ClientID)
			.Add("response_type", "code")
			.Add("redirect_uri", this.Configuration.RedirectURI)
			.Add("scope", Scopes)
			.ToString();

		return uri.Uri;
	}

	protected Uri GetTokenUri() {
		UriBuilder uri = new(TokenUri);

		uri.Query = new QueryString()
			.Add("client_id", this.Configuration.ClientID)
			.Add("client_secret", this.Configuration.ClientSecret)
			.Add("redirect_uri", this.Configuration.RedirectURI)
			.Add("grant_type", "authorization_code")
			.ToString();

		return uri.Uri;
	}
}

public class GoogleOAuthClient : OAuthClient {
	private const string AuthUri = "https://accounts.google.com/o/oauth2/auth";
	private const string TokenUri = "https://oauth2.googleapis.com/token";
	private const string Scopes = "openid email";

	private readonly OAuthProviderConfiguration Configuration;

	public GoogleOAuthClient(OAuthProviderConfiguration config) : base(config, AuthUri, TokenUri, Scopes) {
		this.Configuration = config;
	}
}