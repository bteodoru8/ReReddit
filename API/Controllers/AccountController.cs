using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace API.Controllers;

/// <summary>
/// Controller for account-related operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly string _accessToken;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RedditApiService _redditApiService;

    public AccountController(IHttpContextAccessor httpContextAccessor, RedditApiService redditApiService)
    {
        _httpContextAccessor = httpContextAccessor;
        _accessToken = httpContextAccessor.HttpContext.Session.GetString("AccessToken");
        _redditApiService = redditApiService;
    }

    /// <summary>
    /// Retrieves private messages from Reddit API by recipient username.
    /// </summary>
    /// <param name="recipientUsername">Recipient username.</param>
    /// <returns>JSON response containing the private messages.</returns>
    [HttpGet("private-messages/{recipientUsername}")]
    public async Task<IActionResult> GetPrivateMessagesByRecipient(string recipientUsername)
    {
        try
        {
            // Logic to retrieve private messages from Reddit API by recipient username
            var privateMessages = await _redditApiService.GetPrivateMessagesByRecipient(_accessToken, recipientUsername);
            return Ok(privateMessages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves private messages from Reddit API that are read or unread.
    /// </summary>
    /// <param name="read">Filter for read or unread messages (optional).</param>
    /// <returns>JSON response containing the private messages.</returns>
    [HttpGet("private-messages")]
    public async Task<IActionResult> GetPrivateMessages([FromQuery] bool? read = null)
    {
        try
        {
            if (read != null) {
                if (read == true) {
                    var private_messages = await _redditApiService.GetReadMessages(_accessToken);
                    return Ok(private_messages);
                } else {
                    var private_messages = await _redditApiService.GetUnreadMessages(_accessToken);
                }
            }
            // Logic to retrieve private messages from Reddit API
            var privateMessages = await _redditApiService.GetPrivateMessages(_accessToken);
            return Ok(privateMessages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds a user to the trusted users list.
    /// </summary>
    /// <param name="username">Username of the user to add.</param>
    /// <returns>JSON response indicating the success or failure of the operation.</returns>
    [HttpPost("trusted-users/{username}")]
    public IActionResult AddToTrustedUsers(string username)
    {
        try
        {
            // Logic to add the specified user to the trusted users list
            _redditApiService.AddToTrustedUsers(_accessToken, username);
            return Ok($"Added user '{username}' to the trusted users list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Removes an user from the trusted users list.
    /// </summary>
    /// <param name="username">Username of the user to remove</param>
    /// <returns>JSON response indicating the success of failure of the operation.</returns>
    [HttpDelete("trusted-users/{username}")]
    public IActionResult RemoveFromTrustedUsers(string username)
    {
        try
        {
            // Logic to remove the specified user from the trusted users list
            _redditApiService.RemoveFromTrustedUsers(_accessToken, username);
            return Ok($"Removed user '{username}' from the trusted users list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds an user to the friends list.
    /// </summary>
    /// <param name="username">Username of the user to add.</param>
    /// <returns>JSON response indicating the success or failure of the operation.</returns>
    [HttpPost("friends/{username}")]
    public IActionResult AddToFriends(string username)
    {
        try
        {
            // Logic to add the specified user to the friends list
            _redditApiService.AddToFriends(_accessToken, username);
            return Ok($"Added user '{username}' to the friends list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Removes a user from the friends list.
    /// </summary>
    /// <param name="username">Username of the user to remove.</param>
    /// <returns>JSON response indicating the success or failure of the operation.</returns>
    [HttpDelete("friends/{username}")]
    public IActionResult RemoveFromFriends([FromQuery]string username)
    {
        try
        {
            // Logic to remove the specified user from the friends list
            _redditApiService.RemoveFromFriends(_accessToken, username);
            return Ok($"Removed user '{username}' from the friends list");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the list of friends.
    /// </summary>
    /// <returns>JSON response containing the list of friends.</returns>
    [HttpGet("friends")]
    public IActionResult GetFriendsList()
    {
        try
        {
            // Logic to retrieve the list of friends
            var friendsList = _redditApiService.GetFriendsList(_accessToken);
            return Ok(friendsList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the list of trusted users.
    /// </summary>
    /// <returns>JSON response containing the list of trusted users.</returns>
    [HttpGet("trusted-users")]
    public IActionResult GetTrustedUsersList()
    {
        try
        {
            // Logic to retrieve the list of trusted users
            var trustedUsersList = _redditApiService.GetTrustedUsersList(_accessToken);
            return Ok(trustedUsersList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

