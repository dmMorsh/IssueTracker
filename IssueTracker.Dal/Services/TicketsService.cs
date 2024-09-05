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
            .Include(ticket => ticket.creator)
            .Include(ticket => ticket.executor)
            .Include(ticket => ticket.comments.OrderByDescending(c => c.id))
            .OrderBy(item => item.id)
            .AsNoTracking()
            .ToListAsync();
        return tickets;
    }

    private IQueryable<TicketDto> MakeDto(IQueryable<Ticket> request)
    {
        return request.Select(ticket => new TicketDto
        {
            id = ticket.id,
            title = ticket.title,
            description = ticket.description,
            creator = ticket.creator.UserName,
            creatorId = ticket.creator.Id.ToString(),
            executor = ticket.executor != null ? ticket.executor.UserName : "",
            executorId = ticket.executor != null ? ticket.executor.Id.ToString() : new Guid().ToString(),
            createDate = ticket.createDate,
            dueDate = ticket.dueDate,
            issueType = ticket.issueType,
            priority = ticket.priority,
            space = ticket.space,
            status = ticket.status,
            updatedDate = ticket.updatedDate,
            executionList = ticket.executionList
                .Select(el => new ExecutionListDto
                {
                    ticketId = el.TicketId,
                    userId = el.UserId,
                    userName = el.User.UserName
                })
                .ToList(),
            watchList = ticket.watchList
                .Select(wl => new WatchListDto
                {
                    ticketId = wl.TicketId,
                    userId = wl.UserId,
                    userName = wl.User.UserName
                })
                .ToList(),
            comments = ticket.comments
                .OrderByDescending(c => c.id)
                .Select(comment => new TicketCommentDto
                {
                    id = comment.id,
                    description = comment.description,
                    createDate = comment.createDate,
                    ticketId = comment.ticketId,
                    creator = comment.creator,
                    edited = comment.edited
                })
                .ToList()
        });
    }
    
    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetPage(int page, int pageSize)
    {
        var tickets = _context.tickets
            .OrderBy(item => item.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);    
        return new Tuple<IEnumerable<TicketDto>, int>( await ticketDtos.ToListAsync()
            , await _context.tickets.CountAsync());
    }

    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetWatchingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.watchList
            .Where(i => i.UserId.Equals(userGuid)).Include(w => w.Ticket)
            .Select(t => t.Ticket)
            .OrderBy(item => item.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);    
        return new Tuple<IEnumerable<TicketDto>, int>( await ticketDtos.ToListAsync()
            , await _context.watchList.Where(i => i.UserId.Equals(userGuid)).CountAsync());
    }

    public async Task<Tuple<IEnumerable<TicketDto>, int>> GetExecutingPage(Guid userGuid, int page, int pageSize)
    {
        var tickets = _context.executionList
            .Where(i => i.UserId.Equals(userGuid))
            .Select(t => t.Ticket)
            .OrderBy(item => item.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
        var ticketDtos = MakeDto(tickets);    
        return new Tuple<IEnumerable<TicketDto>, int>( await ticketDtos.ToListAsync()
            , await _context.executionList.Where(i => i.UserId.Equals(userGuid)).CountAsync());
    }

    public async Task<TicketDto> Add(TicketDto ticketDto)
    {
        Ticket ticket = _mapper.Map<Ticket>(ticketDto);
        ApplicationUser? _creator = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.creatorId));
        if (ticket == null || _creator == null)
            return null;
        if (ticketDto.executorId != null && ticketDto.executorId != "")
        {
            ApplicationUser? _executor = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.executorId));
            if (_executor == null)
            ticket.executor = _executor;
        }
        ticket.creator = _creator;
        await _context.tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        var _itemDto = _mapper.Map<TicketDto>(ticket);
        return _itemDto;
    }

    public async Task<bool> Update(int id, TicketDto ticketDto)
    {
        var _ticket = await _context.tickets
            .Include(i => i.executionList)
            .Include(i => i.watchList)
            .FirstOrDefaultAsync(o => o.id == id);
        if(_ticket == null) return false;
        var ticket = _mapper.Map<Ticket>(ticketDto);
        ApplicationUser? _creator = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.creatorId));
        if (ticket == null || _creator == null)
            return false;
        if (ticketDto.executorId != null && ticketDto.executorId != "")
        {
            ApplicationUser? _executor = await _context.Users.FirstOrDefaultAsync(u => u.Id == new Guid(ticketDto.executorId));
            if (_executor == null)
            ticket.executor = _executor;
        }
        ticket.creator = _creator;
        _mapper.Map(ticket, _ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var ticket = await _context.tickets.FirstOrDefaultAsync(o => o.id == id);
        _context.tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

}