using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _context;
    public ChatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetByUserId(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return null;

        var chats = await _context.Chats
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
                lastMessage = _context.Messages
                    .Where(m => m.ChatId == chat.Id)
                    .OrderByDescending(m => m.DateSent)
                    .Select(m => m.Message)
                    .FirstOrDefault()
            })
            .Where(c => c.lastMessage != null)
            .ToListAsync();
        return chats;
    }

    public async Task<int?> GetPersonalChatId(Guid userId, Guid secondUserId)
    {
        var chats = await _context.Chats
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
                await _context.Chats.AddAsync(newChat);
                await _context.SaveChangesAsync();
                return newChat.Id;
            }
            return null;
        }
        return chats[0].Id;
    }

    public async Task<IEnumerable<MiniUser>> GetGuysByChatId(int chatId)
    {
        var users = await _context.Chats
        .Where(chat => chat.Id == chatId)
        .SelectMany(chat => chat.Users)
        .Select(x => new MiniUser { Id = x.Id, Username = x.UserName })
        .ToListAsync();
        return users;
    }

    public async Task AddAsync(Chat chat)
    {
        await _context.Chats.AddAsync(chat);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var chat = await _context.Chats.FindAsync(id);
        if (chat != null)
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }
    }
}