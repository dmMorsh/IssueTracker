using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Infrastructure.Repositories;

public class FriendSubscriptionRepository : IFriendSubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public FriendSubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MiniUser>> GetSubscriptions(Guid userId)
    {
        var subscriptions = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Subscriptions)
            .SelectMany(u => u.Subscriptions)
            .Select(x => new MiniUser
            {
                Id = x.Id,
                Username = x.UserName
            })
            .ToListAsync();
        return subscriptions;
    }

    public async Task<IEnumerable<MiniUser>> GetFriendRequests(Guid userId)
    {
        var friendRequests = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.FriendRequests)
            .SelectMany(u => u.FriendRequests)
            .Select(x => new MiniUser
            {
                Id = x.Id,
                Username = x.UserName
            })
            .ToListAsync();
        return friendRequests;
    }

    public async Task Subscribe(Guid userId, Guid friendId)
    {
        var ourUser = await _context.Users
            .Include(u => u.Friends)
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var friend = await _context.Users
            .Include(f => f.Friends)
            .Include(f => f.FriendRequests)
            .FirstOrDefaultAsync(u => u.Id == friendId);

        if (ourUser == null
        || friend == null
        || ourUser.Friends.Contains(friend)
        || friend.Friends.Contains(ourUser))
        {
            return;
        }

        if (!ourUser.Subscriptions.Contains(friend))
            ourUser.Subscriptions.Add(friend);

        if (!friend.FriendRequests.Contains(ourUser))
            friend.FriendRequests.Add(ourUser);

        await _context.SaveChangesAsync();
    }

    public async Task Unsubscribe(Guid userId, Guid friendId)
    {
        var ourUser = await _context.Users
            .Include(u => u.Friends)
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var friend = await _context.Users
            .Include(f => f.Friends)
            .Include(f => f.FriendRequests)
            .FirstOrDefaultAsync(u => u.Id == friendId);

        if (ourUser == null || friend == null)
            return;

        if (ourUser.Subscriptions.Contains(friend))
            ourUser.Subscriptions.Remove(friend);

        if (friend.FriendRequests.Contains(ourUser))
            friend.FriendRequests.Remove(ourUser);

        await _context.SaveChangesAsync();
    }
}