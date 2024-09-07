namespace IssueTracker.Dal.Models;
public class Ticket
{
    public int id { get; set; }
    public Space? space { get; set; }
    public required string title { get; set; }
    public string? description { get; set; }

    public required ApplicationUser creator { get; set; }
    public ApplicationUser? executor { get; set; }

    public DateTime createDate { get; set; }
    public DateTime updatedDate { get; set; }
    public DateTime dueDate { get; set; }

    public typeOfIssue issueType { get; set; }
    public statusOfTask status { get; set; }
    public priorityOfTask priority { get; set; }

    public ICollection<ExecutionList>? executionList { get; set; }
    public ICollection<WatchList>? watchList { get; set; }
    public virtual ICollection<TicketComment> comments { get; set; } = new List<TicketComment>();
}

public class TicketDto
{
    public int id { get; set; }
    public Space? space { get; set; }
    public required string title { get; set; }
    public string? description { get; set; }

    public required string creatorId { get; set; }
    public string? executorId { get; set; } = "";
    public required string creator { get; set; }
    public string? executor { get; set; } = "";

    public DateTime createDate { get; set; }
    public DateTime updatedDate { get; set; }
    public DateTime dueDate { get; set; }

    public typeOfIssue issueType { get; set; }
    public statusOfTask status { get; set; }
    public priorityOfTask priority { get; set; }

    public ICollection<ExecutionListDto>? executionList { get; set; }
    public ICollection<WatchListDto>? watchList { get; set; }
    public ICollection<TicketCommentDto> comments { get; set; } = new List<TicketCommentDto>();
}