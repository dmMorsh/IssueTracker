using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class FriendSubscriptionService
{
    private readonly IFriendSubscriptionRepository _friendSubscriptionRepository;

    public FriendSubscriptionService(IFriendSubscriptionRepository friendSubscriptionRepository)
    {
        _friendSubscriptionRepository = friendSubscriptionRepository;
    }

    public Task<IEnumerable<MiniUser>> GetSubscriptions(Guid userId)
    {
        return _friendSubscriptionRepository.GetSubscriptions(userId);
    }

    public Task<IEnumerable<MiniUser>> GetFriendRequests(Guid userId)
    {
        return _friendSubscriptionRepository.GetFriendRequests(userId);
    }

    public async Task<bool> Subscribe(Guid userId, Guid friendId)
    {
        await _friendSubscriptionRepository.Subscribe(userId, friendId);
        return true;
    }

    public async Task<bool> Unsubscribe(Guid userId, Guid friendId)
    {
        await _friendSubscriptionRepository.Unsubscribe(userId, friendId);
        return true;
    }
}