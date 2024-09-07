using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;

namespace IssueTracker.Dal.Services;

public class MessagesService
{
    private readonly ApplicationDbContext _context;

    public MessagesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserMessage>> Get()
    {
        var messages = await _context.messages
            .OrderBy(item => item.Id)
            .ToListAsync();
        return messages;
    }

    public async Task<IEnumerable<UserMessage>> getById(int id)
    {
        var messages = await _context.messages
            .Where(m => m.ChatId == id)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
        return messages;
    }

    public async Task<bool> Add(UserMessage item)
    {
        await _context.messages.AddAsync(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(int id, UserMessage item)
    {
        var _item = await _context.messages.FirstOrDefaultAsync(o => o.Id == id);
        if (_item == null)
            return false;
        _context.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var item = await _context.messages.FirstOrDefaultAsync(o => o.Id == id);
        if (item == null)
            return false;
        _context.messages.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}