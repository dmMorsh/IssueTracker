using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;

namespace IssueTracker.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        var tickets = await _context.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.Creator)
            .Include(ticket => ticket.Executor)
            .Include(ticket => ticket.Comments.OrderByDescending(c => c.Id))
            .OrderBy(item => item.Id)
            .ToListAsync();
        return tickets;
    }

    public async Task<IEnumerable<Ticket>> GetPage(int page, int pageSize)
    {
        var tickets = _context.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.Creator)
            .Include(ticket => ticket.Executor)
            .Include(ticket => ticket.ExecutionList)
            .Include(ticket => ticket.WatchList)
            .Include(ticket => ticket.Comments.OrderByDescending(c => c.Id))
            .OrderBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize) 
            .ToListAsync();
        return await tickets;
    }

    public async Task<int> GetPageCount()
    {
        return await _context.Tickets.CountAsync();
    }

    public async Task<IEnumerable<Ticket>> GetWatchingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.WatchList
            .AsNoTracking()
            .Where(i => i.UserId.Equals(userGuid))
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Creator)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Executor)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.ExecutionList)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.WatchList)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Comments.OrderByDescending(c => c.Id))
            .Select(t => t.Ticket)
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return await tickets;
    }

    public async Task<int> GetWatchingPageCount(Guid userGuid)
    {
        return await _context.WatchList.Where(i => i.UserId.Equals(userGuid)).CountAsync();
    }

    public async Task<IEnumerable<Ticket>> GetExecutingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.ExecutionList
            .AsNoTracking()
            .Where(i => i.UserId.Equals(userGuid))
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Creator)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Executor)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.ExecutionList)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.WatchList)
            .Include(w => w.Ticket)
                .ThenInclude(t => t.Comments.OrderByDescending(c => c.Id))
            .Select(t => t.Ticket)
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return await tickets;
    }

    public async Task<int> GetExecutingPageCount(Guid userGuid)
    {
        return await _context.ExecutionList.Where(i => i.UserId.Equals(userGuid)).CountAsync();
    }

    public async Task<Guid?> GetExecutorIdByTicketId(int ticketId)
    {
        var executorId = _context.Tickets.Where(t => t.Id == ticketId).Select(x=>x.ExecutorId).FirstOrDefaultAsync();
        return await executorId;
    }
    public async Task AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket != null)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}