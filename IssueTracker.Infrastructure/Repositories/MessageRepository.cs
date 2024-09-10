using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;

namespace IssueTracker.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserMessage>> GetAllByChatIdAsync(int chatId)
    {
        var messages = await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.Id)
            .ToListAsync();
        return messages;
    }

    public async Task AddAsync(UserMessage userMessage)
    {
        await _context.Messages.AddAsync(userMessage);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserMessage userMessage)
    {
        _context.Messages.Update(userMessage);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var userMessage = await _context.Messages.FindAsync(id);
        if (userMessage != null)
        {
            _context.Messages.Remove(userMessage);
            await _context.SaveChangesAsync();
        }
    }
}