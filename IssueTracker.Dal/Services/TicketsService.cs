using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;
using AutoMapper;

namespace IssueTracker.Dal.Services;

public class TicketsService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public TicketsService(
        ApplicationDbContext context
        , IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Ticket>> GetAll()
    {
        var tickets = await _context.tickets
            .Include(ticket => ticket.Creator)
            .Include(ticket => ticket.Executor)
            .Include(ticket => ticket.Comments.OrderByDescending(c => c.Id))
            .OrderBy(item => item.Id)
            .AsNoTracking()
            .ToListAsync();
        return tickets;
    }

    private IQueryable<TicketDto> MakeDto(IQueryable<Ticket> request)
    {
        return request.Select(ticket => new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Creator = ticket.Creator.UserName,
            CreatorId = ticket.Creator.Id.ToString(),
            Executor = ticket.Executor != null ? ticket.Executor.UserName : "",
            ExecutorId = ticket.Executor != null ? ticket.Executor.Id.ToString() : new Guid().ToString(),
            CreateDate = ticket.CreateDate,
            DueDate = ticket.DueDate,
            IssueType = ticket.IssueType,
            Priority = ticket.Priority,
            Space = ticket.Space,
            Status = ticket.Status,
            UpdatedDate = ticket.UpdatedDate,
            ExecutionList = ticket.ExecutionList
                .Select(el => new ExecutionListDto
                {
                    TicketId = el.TicketId,
                    UserId = el.UserId,
                    UserName = el.User.UserName
                })
                .ToList(),
            WatchList = ticket.WatchList
                .Select(wl => new WatchListDto
                {
                    TicketId = wl.TicketId,
                    UserId = wl.UserId,
                    UserName = wl.User.UserName
                })
                .ToList(),
            Comments = ticket.Comments
                .OrderByDescending(c => c.Id)
                .Select(comment => new TicketCommentDto
                {
                    Id = comment.Id,
                    Description = comment.Description,
                    CreateDate = comment.CreateDate,
                    TicketId = comment.TicketId,
                    Creator = comment.Creator,
                    Edited = comment.Edited
                })
                .ToList()
        });
    }

    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetPage(int page, int pageSize)
    {
        var tickets = _context.tickets
            .OrderBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);
        return new Tuple<IEnumerable<TicketDto>, int>(await ticketDtos.ToListAsync(),
            await _context.tickets.CountAsync());
    }

    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetWatchingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.watchList
            .Where(i => i.UserId.Equals(userGuid)).Include(w => w.Ticket)
            .Select(t => t.Ticket)
            .OrderBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);
        return new Tuple<IEnumerable<TicketDto>, int>(await ticketDtos.ToListAsync(),
            await _context.watchList.Where(i => i.UserId.Equals(userGuid)).CountAsync());
    }

    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetExecutingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.executionList
            .Where(i => i.UserId.Equals(userGuid))
            .Select(t => t.Ticket)
            .OrderBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);
        return new Tuple<IEnumerable<TicketDto>, int>(await ticketDtos.ToListAsync(),
            await _context.executionList.Where(i => i.UserId.Equals(userGuid)).CountAsync());
    }

    public async Task<TicketDto> Add(TicketDto ticketDto)
    {
        Ticket ticket = _mapper.Map<Ticket>(ticketDto);
        ApplicationUser? creator = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.CreatorId));
        if (ticket == null || creator == null)
            return null;
        if (ticketDto.ExecutorId != null && ticketDto.ExecutorId != "")
        {
            ApplicationUser? _executor = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.ExecutorId));
            if (_executor != null)
                ticket.Executor = _executor;
        }
        ticket.Creator = creator;
        await _context.tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        var _itemDto = _mapper.Map<TicketDto>(ticket);
        return _itemDto;
    }

    public async Task<bool> Update(int id, TicketDto ticketDto)
    {
        var _ticket = await _context.tickets
            .Include(i => i.ExecutionList)
            .Include(i => i.WatchList)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (_ticket == null)
            return false;
        var ticket = _mapper.Map<Ticket>(ticketDto);
        ApplicationUser? creator = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.CreatorId));
        if (ticket == null || creator == null)
            return false;
        if (ticketDto.ExecutorId != null && ticketDto.ExecutorId != "")
        {
            ApplicationUser? _executor = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.ExecutorId));
            if (_executor != null)
                ticket.Executor = _executor;
        }
        ticket.Creator = creator;
        _mapper.Map(ticket, _ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var ticket = await _context.tickets.FirstOrDefaultAsync(o => o.Id == id);
        _context.tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }
}