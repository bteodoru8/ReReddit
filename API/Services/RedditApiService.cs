using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Models;

namespace API.Services;

/// <summary>
/// This class provides the implementation for interacting with the Reddit API.
/// </summary>
public class RedditApiService
{
    private const string RedditApiBaseUrl = "https://oauth.reddit.com";
    private readonly HttpClient _httpClient;

    public RedditApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves private messages from the Reddit API by recipient username.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <param name="recipientUsername">Username of the recipient.</param>
    /// <returns>List of private messages.</returns>
    public async Task<List<PrivateMessage>> GetPrivateMessagesByRecipient(string accessToken, string recipientUsername)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/message/messages?filter=unread&user=<username>private_messages/{recipientUsername}";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the GET request and parse the response
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var privateMessages = JsonSerializer.Deserialize<List<PrivateMessage>>(responseContent);

        return privateMessages;
    }
    /// <summary>
    /// Retrieves all private messages from the Reddit API.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>List of private messages.</returns>
    public async Task<List<PrivateMessage>> GetPrivateMessages(string accessToken) {
        string apiUrl = $"{RedditApiBaseUrl}/message/inbox";

        var client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        var response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var messageList = JsonSerializer.Deserialize<PrivateMessageList>(responseContent);

        return messageList.Data.Children.Select(m => m.Data).ToList();
    }

    /// <summary>
    /// Retrieves all read private messages from the Reddit API.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>List of private messages.</returns>
    public async Task<List<PrivateMessage>> GetReadMessages(string accessToken){
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/me/messages?filter=read";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the GET request and parse the response
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var messageList = JsonSerializer.Deserialize<PrivateMessageList>(responseContent);

        return messageList.Data.Children.Select(m => m.Data).ToList();
    }

    /// <summary>
    /// Retroeves all unread private messages from the Reddit API.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>List of private messages.</returns>
    public async Task<List<PrivateMessage>> GetUnreadMessages(string accessToken){
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/me/messages?filter=unread";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the GET request and parse the response
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var messageList = JsonSerializer.Deserialize<PrivateMessageList>(responseContent);

        return messageList.Data.Children.Select(m => m.Data).ToList();
    }

    /// <summary>
    /// Adds an user to the trusted-users list.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <param name="username">The username of the user to be added to the list.</param>
    public void AddToTrustedUsers(string accessToken, string username)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/api/v1/me/trusted";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the POST request to add the user to the trusted users list
        var response = _httpClient.PostAsync(apiUrl, null).Result;
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Removes an user from the trusted-users list.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <param name="username">The username of the user to be removed from the list.</param>
    public void RemoveFromTrustedUsers(string accessToken, string username)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/api/v1/me/trusted/{username}";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the POST request to remove the user from the trusted users list
        var response = _httpClient.PostAsync(apiUrl, null).Result;
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Adds an user as a friend.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <param name="username">The username of the user to be added to the list.</param>
    public void AddToFriends(string accessToken, string username)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/api/v1/me/friends/{username}";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the POST request to add the user to the friends list
        var response = _httpClient.PostAsync(apiUrl, null).Result;
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Removes an user from friends.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <param name="username">The username of the user to be added to the list.</param>
    public void RemoveFromFriends(string accessToken, string username)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/api/v1/me/friends/{username}";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the POST request to remove the user from the friends list
        var response = _httpClient.PostAsync(apiUrl, null).Result;
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Gets the list of friends.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>JSON string representing the list of friends.</returns>
    public List<string> GetFriendsList(string accessToken)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/api/v1/me/friends";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the GET request and parse the response
        var response = _httpClient.GetAsync(apiUrl).Result;
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var friendsList = JsonSerializer.Deserialize<List<string>>(responseContent);

        return friendsList;
    }

    /// <summary>
    /// Gets the list of trusted users
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>JSON string representing the lsit of friends.</returns>
    public List<string> GetTrustedUsersList(string accessToken)
    {
        // Construct the API endpoint URL
        var apiUrl = $"{RedditApiBaseUrl}/prefs/trusted";

        // Set the Authorization header with the access token
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        // Send the GET request and parse the response
        var response = _httpClient.GetAsync(apiUrl).Result;
        response.EnsureSuccessStatusCode();
        var responseContent = response.Content.ReadAsStringAsync().Result;
        var trustedUsersList = JsonSerializer.Deserialize<List<string>>(responseContent);

        return trustedUsersList;
    }
}