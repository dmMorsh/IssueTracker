using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;

namespace IssueTracker.Dal.Services;

public class FriendsSubscriptionsService 
{
    private readonly ApplicationDbContext _context;
    
    public FriendsSubscriptionsService(ApplicationDbContext context)
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
                id = x.Id,
                username = x.UserName
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
                id = x.Id,
                username = x.UserName
            })
            .ToListAsync();
        return friendRequests;
    }

    public async Task<bool> Subscribe(Guid userId, Guid friendId)
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
            return false;

        if (!ourUser.Subscriptions.Contains(friend))
            ourUser.Subscriptions.Add(friend);

        if (!friend.FriendRequests.Contains(ourUser))
            friend.FriendRequests.Add(ourUser);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Unsubscribe(Guid userId, Guid friendId)
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
            return false;

        if (ourUser.Subscriptions.Contains(friend))
            ourUser.Subscriptions.Remove(friend);

        if (friend.FriendRequests.Contains(ourUser))
            friend.FriendRequests.Remove(ourUser);

        await _context.SaveChangesAsync();
        return true;
    }

}