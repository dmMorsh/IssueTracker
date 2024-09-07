namespace IssueTracker.Dal.Models;
public class Ticket
{
    public int Id { get; set; }
    public Space? Space { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public required ApplicationUser Creator { get; set; }
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

public class TicketDto
{
    public int Id { get; set; }
    public Space? Space { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public required string CreatorId { get; set; }
    public string? ExecutorId { get; set; } = "";
    public required string Creator { get; set; }
    public string? Executor { get; set; } = "";

    public DateTime CreateDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime DueDate { get; set; }

    public typeOfIssue IssueType { get; set; }
    public statusOfTask Status { get; set; }
    public priorityOfTask Priority { get; set; }

    public ICollection<ExecutionListDto>? ExecutionList { get; set; }
    public ICollection<WatchListDto>? WatchList { get; set; }
    public ICollection<TicketCommentDto> Comments { get; set; } = new List<TicketCommentDto>();
}