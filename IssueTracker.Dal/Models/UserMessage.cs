namespace IssueTracker.Dal.Models;
public class UserMessage
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public required string Sender { get; set; }
    public required string Message { get; set; }
    public DateTime DateSent { get; set; }
}