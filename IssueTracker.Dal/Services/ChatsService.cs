using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;

namespace IssueTracker.Dal.Services;

public class ChatService
{
    private readonly ApplicationDbContext _context;
    public ChatService(ApplicationDbContext context)
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
                chat.Id,
                info = chat.IsPersonal
                    ? chat.Users
                        .Where(u => u.Id != userId)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                    : chat.Info == ""
                        ? chat.Info
                        : string.Join(", ", chat.Users.Select(u => u.UserName)),
                lastMessage = _context.messages
                    .Where(m => m.ChatId == chat.Id)
                    .OrderByDescending(m => m.DateSent)
                    .Select(m => m.Message)
                    .FirstOrDefault()
            })
            .Where(c => c.lastMessage != null)
            .ToListAsync();
        return chats;
    }

    public async Task<int?> getPersonalChatId(Guid userId, Guid secondUserId)
    {
        var chats = await _context.chats
            .Where(c => c.Users.Any(u => u.Id == userId)
                    && c.IsPersonal
                    && c.Users.Any(u => u.Id == secondUserId))
            .ToListAsync();

        if (chats.Count() == 0)
        {
            var per1 = await _context.Users.FindAsync(userId);
            var per2 = await _context.Users.FindAsync(secondUserId);

            if (per1 != null && per2 != null)
            {
                var newChat = new Chat
                {
                    IsPersonal = true,
                    Users = new ApplicationUser[] { per1, per2 }
                };
                await _context.chats.AddAsync(newChat);
                await _context.SaveChangesAsync();
                return newChat.Id;
            }
            return null;
        }
        return chats[0].Id;
    }

    public async Task<IEnumerable<MiniUser>> getGuysById(int id)
    {
        var users = await _context.chats
        .Where(chat => chat.Id == id)
        .SelectMany(chat => chat.Users) // Выборка всех пользователей из найденного чата
        .Select(x => new MiniUser { Id = x.Id, Username = x.UserName })
        .ToListAsync();
        return users;
    }

    public async Task<bool> Add(Chat Chat)
    {
        await _context.chats.AddAsync(Chat);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(int id)
    {
        var item = await _context.chats.FirstOrDefaultAsync(o => o.Id == id);
        if (item == null)
            return false;
        _context.chats.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}