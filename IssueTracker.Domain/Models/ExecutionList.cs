namespace IssueTracker.Domain.Models;

public class ExecutionList
{
    public required Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public required int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}