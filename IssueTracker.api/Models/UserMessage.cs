namespace IssueTracker.Models;
public class UserMessage
{
    public int id { get; set; }
    public int chatId { get; set; }
    public required string sender { get; set; }
    public required string message { get; set; }
    public DateTime dateSent { get; set; }
}