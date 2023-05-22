using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace API.Services;

public class RedditOAuthService {
   private const string AuthorizationUrl = "https://www.reddit.com/api/v1/authorize";
   private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";

   private readonly string _clientId;
   private readonly string _clientSecret;
   private readonly string _redirectUri;

   public RedditOAuthService(string clientId, string clientSecret, string redirectUri) {
      _clientId = clientId;
      _clientSecret = clientSecret;
      _redirectUri = redirectUri;
   }

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

   public async Task<string> ExchangeAuthorizationCodeForAccessToken(string code) {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/113.0");

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

   private string BuildAuthHeaderValue() {
      string credentials = $"{_clientId}:{_clientSecret}";
      byte[] credentialsBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
      return Convert.ToBase64String(credentialsBytes);
   }
}