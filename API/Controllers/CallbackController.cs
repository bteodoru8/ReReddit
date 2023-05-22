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

   public CallbackController(RedditOAuthService oAuthService) {
      _oauthService = oAuthService;
   }

   [HttpGet]
   public IActionResult RedirectToAuthorizationUrl() {
      string state = "RANDOM_STRING";
      string duration = "temporary";
      string scope = "identity privatemessages";

      string authorizationUrl = _oauthService.GetAuthorizationLink(state, duration, scope);
      Console.WriteLine(authorizationUrl);
      
      return Redirect(authorizationUrl);
   }

   [HttpGet("redirect")]
   public async Task<IActionResult> HandleCallback(string code, string state) {
      try {
         // Verify the state parameter to ensure it matches the one sent in the initial authorization request
         // Exchange the authorization code for an access token
         string accessToken = await _oauthService.ExchangeAuthorizationCodeForAccessToken(code);

         Console.WriteLine(accessToken);
         HttpContext.Session.SetString("AccessToken", accessToken);
         return Ok(accessToken);
         // Process the access token as needed (e.g., store it in a session, authenticate the user, etc.)
         // Redirect to a success page or perform other actions
         return View("Success");
      } catch (Exception ex) {
         // Handle the error gracefully
         ViewBag.ErrorMessage = ex.Message;
         return View("Error");
      }
   }
}
