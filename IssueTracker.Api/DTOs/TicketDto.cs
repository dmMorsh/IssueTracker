namespace IssueTracker.Api.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public int? SpaceId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public required Guid CreatorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public required string Creator { get; set; }
    public string? Executor { get; set; } = "";

    public DateTime CreateDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime DueDate { get; set; }

    public short IssueType { get; set; }
    public short Status { get; set; }
    public short Priority { get; set; }

    public ICollection<ExecutionListDto>? ExecutionList { get; set; }
    public ICollection<WatchListDto>? WatchList { get; set; }
    public ICollection<TicketCommentDto> Comments { get; set; } = new List<TicketCommentDto>();
}