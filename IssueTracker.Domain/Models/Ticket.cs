namespace IssueTracker.Domain.Models;

public class Ticket
{
    public int Id { get; set; }
    public int? SpaceId { get; set; }
    public Space? Space { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public required Guid CreatorId { get; set; }
    public required ApplicationUser Creator { get; set; }

    public required Guid? ExecutorId { get; set; }
    public ApplicationUser? Executor { get; set; }

    public DateTime CreateDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime DueDate { get; set; }

    public typeOfIssue IssueType { get; set; }
    public statusOfTask Status { get; set; }
    public priorityOfTask Priority { get; set; }

    public ICollection<ExecutionList>? ExecutionList { get; set; }
    public ICollection<WatchList>? WatchList { get; set; }
    public virtual ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
}