using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;

namespace IssueTracker.Dal.Services;

public class ChatsService 
{
    private readonly ApplicationDbContext _context;
    public ChatsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> Get(Guid userId)
    {
        var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null) 
            return null;

        var chats = await _context.chats
            .Where(c => c.Users.Contains(user))
            .Select(chat => new
            {
                chat.id, 
                info = chat.isPersonal
                    ? chat.Users
                        .Where(u => u.Id != userId)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                    : chat.info == ""
                        ? chat.info
                        : string.Join(", ", chat.Users.Select(u => u.UserName)),
                lastMessage = _context.messages
                    .Where(m => m.chatId == chat.id)
                    .OrderByDescending(m => m.dateSent)
                    .Select(m => m.message)
                    .FirstOrDefault()
            })
            .Where(c=>c.lastMessage != null)
            .ToListAsync();

        return chats;
    }

}