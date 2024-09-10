namespace IssueTracker.Api.DTOs;

public class WatchListDto
{
    public required Guid UserId { get; set; }
    public required int TicketId { get; set; }
    public string? UserName { get; set; }
}