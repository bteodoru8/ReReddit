namespace API.Models;

public class Account {
   public string? AppId { get; set; }
   public string? AppSecret { get; set; }
   public string? AccessToken{ get; set; }
   public string? RefreshToken { get; set; }
   public string? DeviceId { get; set; }
}