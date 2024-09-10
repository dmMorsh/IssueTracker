using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface IFriendSubscriptionRepository
{
    Task<IEnumerable<MiniUser>> GetSubscriptions(Guid userId);
    Task<IEnumerable<MiniUser>> GetFriendRequests(Guid userId);
    Task Subscribe(Guid userId, Guid friendId);
    Task Unsubscribe(Guid userId, Guid friendId);
}