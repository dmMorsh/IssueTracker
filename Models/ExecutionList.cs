namespace IssueTracker.Models;
public class ExecutionList
{
    public required Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public required int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}

public class ExecutionListDto
{
    public required Guid userId { get; set; }
    public required int ticketId { get; set; }
    public string? userName { get; set; }
}