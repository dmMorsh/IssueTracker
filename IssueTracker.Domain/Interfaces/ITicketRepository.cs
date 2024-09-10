using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<IEnumerable<Ticket>> GetPage(int page, int pageSize);
    Task<int> GetPageCount();
    Task<IEnumerable<Ticket>> GetWatchingPage(Guid userGuid, int page, int pageSize);
    Task<int> GetWatchingPageCount(Guid userGuid);
    Task<IEnumerable<Ticket>> GetExecutingPage(Guid userGuid, int page, int pageSize);
    Task<int> GetExecutingPageCount(Guid userGuid);
    Task<Guid?> GetExecutorIdByTicketId(int ticketId);
    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(int id);
}