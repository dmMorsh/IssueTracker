using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Infrastructure.Repositories;

public class FriendRepository : IFriendRepository
{
    private readonly ApplicationDbContext _context;

    public FriendRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MiniUser>> GetAllByUserId(Guid userId)
    {
        var friends = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Friends)
            .SelectMany(u => u.Friends)
            .Select(x => new MiniUser
            {
                Id = x.Id,
                Username = x.UserName
            })
            .ToListAsync();
        return friends;
    }

    public async Task AddAsync(Guid userId, Guid friendId)
    {
        var ourUser = await _context.Users
            .Include(u => u.Friends)
            .Include(u => u.FriendRequests)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var friend = await _context.Users
            .Include(f => f.Friends)
            .Include(f => f.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == friendId);

        if (ourUser == null || friend == null)
            return;

        if (ourUser.FriendRequests.Contains(friend))
            ourUser.FriendRequests.Remove(friend);
        else
            return;

        if (friend.Subscriptions.Contains(ourUser))
            friend.Subscriptions.Remove(ourUser);
        else
            return;

        if (!ourUser.Friends.Contains(friend))
            ourUser.Friends.Add(friend);

        if (!friend.Friends.Contains(ourUser))
            friend.Friends.Add(ourUser);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId, Guid friendId)
    {
        var ourUser = await _context.Users
            .Include(u => u.Friends)
            .Include(u => u.FriendRequests)
            .FirstOrDefaultAsync(u => u.Id == userId);

        var friend = await _context.Users
            .Include(f => f.Friends)
            .Include(f => f.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == friendId);

        if (ourUser == null || friend == null)
            return;

        if (!ourUser.FriendRequests.Contains(friend))
            ourUser.FriendRequests.Add(friend);

        if (!friend.Subscriptions.Contains(ourUser))
            friend.Subscriptions.Add(ourUser);

        if (ourUser.Friends.Contains(friend))
            ourUser.Friends.Remove(friend);

        if (friend.Friends.Contains(ourUser))
            friend.Friends.Remove(ourUser);

        await _context.SaveChangesAsync();
    }
}