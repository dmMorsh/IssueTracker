using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class FriendService
{
    private readonly IFriendRepository _friendRepository;

    public FriendService(IFriendRepository friendRepository)
    {
        _friendRepository = friendRepository;
    }

    public Task<IEnumerable<MiniUser>> GetAllByUserId(Guid userId)
    {
        return _friendRepository.GetAllByUserId(userId);
    }

    public async Task<bool> AddAsync(Guid userId, Guid friendId)
    {
        await _friendRepository.AddAsync(userId, friendId);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid friendId)
    {
        await _friendRepository.DeleteAsync(userId, friendId);
        return true;
    }
}