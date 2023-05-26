using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace API.Services;

/// <summary>
/// Service class to provide OAuth functionality for Reddit API
/// </summary>
public class RedditOAuthService {
   private const string AuthorizationUrl = "https://www.reddit.com/api/v1/authorize";
   private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";

   private readonly string _clientId;
   private readonly string _clientSecret;
   private readonly string _redirectUri;

   /// <summary>
   /// Initializes a new instance of the RedditOAuthService class.
   /// </summary>
   /// <param name="clientId">The client ID obtained from Reddit API.</param>
   /// <param name="clientSecret">The client secret obtained from Reddit API.</param>
   /// <param name="redirectUri">The redirect URI registered with the Reddit app.</param>
   public RedditOAuthService(string clientId, string clientSecret, string redirectUri) {
      _clientId = clientId;
      _clientSecret = clientSecret;
      _redirectUri = redirectUri;
   }
   
   /// <summary>
   /// Gets the authorization link to initiate the OAuth flow.
   /// </summary>
   /// <param name="state">A unique identifier for the client's session.</param>
   /// <param name="duration">The duration of the access token.</param>
   /// <param name="scope">The requested permissions/scopes for the access token.</param>
   /// <returns>The auhtorization link.</returns>
   public string GetAuthorizationLink(string state, string duration, string scope) {
      var queryParams = new Dictionary<string, string> {
         { "client_id", _clientId },
         { "response_type", "code" },
         { "state", state },
         { "redirect_uri", _redirectUri },
         { "duration", duration },
         { "scope", scope }
      };

      var queryString = string.Join("&", queryParams
         .Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));

      return $"{AuthorizationUrl}?{queryString}";
   }

   /// <summary>
   /// Exchanges the authorization code for an access token.
   /// </summary>
   /// <param name="code">The authorization code received from the authorization callback.</param>
   /// <returns>The access token.</returns>
   public async Task<string> ExchangeAuthorizationCodeForAccessToken(string code) {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("User-Agent", "redditdev simple api by u/dontyoudare69");

      var postData = new Dictionary<string, string> {
         { "grant_type", "authorization_code" },
         { "code", code },
         { "redirect_uri", _redirectUri }
      };

      client.DefaultRequestHeaders.Add("Authorization", $"Basic {BuildAuthHeaderValue()}");

      var response = await client.PostAsync(AccessTokenUrl, new FormUrlEncodedContent(postData));
      var responseContent = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode) {
         var jsonDocument = JsonDocument.Parse(responseContent);
         string? accessToken = jsonDocument.RootElement.GetProperty("access_token").GetString();

         return accessToken;
      } else {
         throw new Exception($"Failed to retrieve access token. Response: {responseContent}");
      }
   }

   /// <summary>
   /// Small auxiliary method to build the header value required for authentication in a cool way.
   /// </summary>
   /// <returns> The Header string. </returns>
   private string BuildAuthHeaderValue() {
      string credentials = $"{_clientId}:{_clientSecret}";
      byte[] credentialsBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
      return Convert.ToBase64String(credentialsBytes);
   }
}