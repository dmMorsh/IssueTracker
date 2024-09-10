using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface ICommentRepository
{
    Task AddAsync(TicketComment comment);
    Task UpdateAsync(TicketComment comment);
    Task DeleteAsync(int id);
}