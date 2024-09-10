using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class TicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return _ticketRepository.GetAllAsync();
    }

    public Task<IEnumerable<Ticket>> GetPage(int page, int pageSize)
    {
        return _ticketRepository.GetPage(page, pageSize);
    }

    public Task<int> GetPageCount()
    {
        return _ticketRepository.GetPageCount();
    }

    public Task<IEnumerable<Ticket>> GetWatchingPage(Guid userGuid, int page, int pageSize)
    {
        return _ticketRepository.GetWatchingPage(userGuid, page, pageSize);
    }

    public Task<int> GetWatchingPageCount(Guid userGuid)
    {
        return _ticketRepository.GetWatchingPageCount(userGuid);
    }

    public Task<IEnumerable<Ticket>> GetExecutingPage(Guid userGuid, int page, int pageSize)
    {
        return _ticketRepository.GetExecutingPage(userGuid, page, pageSize);
    }

    public Task<int> GetExecutingPageCount(Guid userGuid)
    {
        return _ticketRepository.GetExecutingPageCount(userGuid);
    }

    public Task<Guid?> GetExecutorIdByTicketId(int ticketId){
        return _ticketRepository.GetExecutorIdByTicketId(ticketId);
    }

    public async Task<bool> AddAsync(Ticket ticket)
    {
        await _ticketRepository.AddAsync(ticket);
        return true;
    }

    public async Task<bool> UpdateAsync(int id,Ticket ticket)
    {
        await _ticketRepository.UpdateAsync(ticket);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _ticketRepository.DeleteAsync(id);
        return true;
    }
}