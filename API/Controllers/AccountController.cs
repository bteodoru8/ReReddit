using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] // Denotes this class is an API Controller
[Route("[account]")] // Denotes the route prefix of this controller to be /account
public class AccountController : Controller {
   private readonly string _accessToken;
   private readonly IHttpContextAccessor _httpContextAccessor;
   public AccountController (IHttpContextAccessor httpContextAccessor) {
      _httpContextAccessor = httpContextAccessor;
      _accessToken = httpContextAccessor.HttpContext.Session.GetString("AccessToken");
   }


}