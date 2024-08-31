namespace IssueTracker.Models;
public class TicketComment
{
    public int id { get; set; }
    public string creator { get; set; } = "";
    public DateTime createDate { get; set; }
    public string description { get; set; } = "";
    public bool edited { get; set; }

    public int ticketId { get; set; }
    public Ticket ticket { get; set; }
}

public class TicketCommentDto
{
    public int id { get; set; }
    public string creator { get; set; } = "";
    public DateTime createDate { get; set; }
    public string description { get; set; } = "";
    public bool edited { get; set; }

    public int ticketId { get; set; }
}