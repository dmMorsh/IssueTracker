using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface IMessageRepository
{
    Task<IEnumerable<UserMessage>> GetAllByChatIdAsync(int chatId);
    Task AddAsync(UserMessage message);
    Task UpdateAsync(UserMessage message);
    Task DeleteAsync(int id);
}