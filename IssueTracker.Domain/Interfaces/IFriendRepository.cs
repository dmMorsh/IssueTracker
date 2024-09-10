using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface IFriendRepository
{
    Task<IEnumerable<MiniUser>> GetAllByUserId(Guid userId);
    Task AddAsync(Guid userId, Guid friendId);
    Task DeleteAsync(Guid userId, Guid friendId);
}