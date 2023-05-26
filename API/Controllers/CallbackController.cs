using API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CallbackController : Controller {
   private const string client_id = "Sh7J7L0YFbDywsABEY11uw";
   private const string secret = "9__bIHJRvhH1sl6MlqSVYJJMXIwzyA";
   private const string redirectUri = "http://localhost:5145/callback/redirect";

   private readonly RedditOAuthService _oauthService;

   /// <summary>
   /// Initializes a new instance of the CallbackController class.
   /// </summary>
   /// <param name="oAuthService">The RedditOAuthService instance for handling OAuth operations.</param>
   public CallbackController(RedditOAuthService oAuthService) {
      _oauthService = oAuthService;
   }

   /// <summary>
   /// Redirects to the authorization URL for initiating the OAuth flow.
   /// </summary>
   /// <returns>An HTTP redirect response to the authorization URL.</returns>
   [HttpGet]
   public IActionResult RedirectToAuthorizationUrl() {
      string state = "RANDOM_STRING";
      string duration = "temporary";
      string scope = "privatemessages mysubreddits read";

      string authorizationUrl = _oauthService.GetAuthorizationLink(state, duration, scope);
      Console.WriteLine(authorizationUrl);
      
      return Redirect(authorizationUrl);
   }

   /// <summary>
   /// Handles the callback from the authorization server after the user authorizes the app.
   /// </summary>
   /// <param name="code">The authorization code received from the authorization server.</param>
   /// <param name="state">The state parameter received from the authorization server.</param>
   /// <returns>An asynchronous task that represents the action result.</returns>
   [HttpGet("redirect")]
   public async Task<IActionResult> HandleCallback(string code, string state) {
      try {
         // Verify the state parameter to ensure it matches the one sent in the initial authorization request
         // Exchange the authorization code for an access token
         string accessToken = await _oauthService.ExchangeAuthorizationCodeForAccessToken(code);

         Console.WriteLine(accessToken);
         HttpContext.Session.SetString("AccessToken", accessToken);
         return View("Success");
      } catch (Exception ex) {
         // Handle the error gracefully
         ViewBag.ErrorMessage = ex.Message;
         return View("Error");
      }
   }
}
