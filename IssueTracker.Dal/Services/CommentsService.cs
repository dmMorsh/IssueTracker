using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;
using AutoMapper;

namespace IssueTracker.Dal.Services;

public class CommentsService 
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public CommentsService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketCommentDto>> GetAll()
    {
        var items = await _context.comments.ToListAsync();
        var ticketCommentDto = _mapper.Map<IEnumerable<TicketComment>, IEnumerable<TicketCommentDto>>(items);
        return ticketCommentDto;
    }

    public async Task<TicketCommentDto> Add(TicketCommentDto itemDto)
    {
        TicketComment item = _mapper.Map<TicketComment>(itemDto);
        await _context.comments.AddAsync(item);
        await _context.SaveChangesAsync();
        var _itemDto = _mapper.Map<TicketCommentDto>(item);
        return _itemDto;
    }

    public async Task<bool> Update(int id, TicketCommentDto itemDto)
    {
        var _item = await _context.comments.FirstOrDefaultAsync(o => o.id == id);
        if(_item == null) return false;
        var item = _mapper.Map<TicketComment>(itemDto);
        _mapper.Map(item, _item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var item = await _context.comments.FirstOrDefaultAsync(o => o.id == id);
        if (item != null) 
        {
            _context.comments.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}