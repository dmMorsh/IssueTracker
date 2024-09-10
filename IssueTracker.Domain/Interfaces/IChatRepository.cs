using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface IChatRepository
{
    Task<IEnumerable<object>> GetByUserId(Guid userId);
    Task<int?> GetPersonalChatId(Guid userId, Guid secondUserId);
    Task<IEnumerable<MiniUser>> GetGuysByChatId(int chatId);
    Task AddAsync(Chat Chat);
    Task DeleteAsync(int id);
}