namespace IssueTracker.Domain.Models;

public class TicketComment
{
    public int Id { get; set; }
    public string Creator { get; set; } = "";
    public DateTime CreateDate { get; set; }
    public string Description { get; set; } = "";
    public bool Edited { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}