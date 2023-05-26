namespace API.Models;

public class PrivateMessage {
    public string Id { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsRead { get; set; }
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public DateTime CreatedAt { get; set; }
}
