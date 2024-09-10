namespace IssueTracker.Api.DTOs;

public class ExecutionListDto
{
    public required Guid UserId { get; set; }
    public required int TicketId { get; set; }
    public string? UserName { get; set; }
}