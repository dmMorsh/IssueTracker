using Microsoft.EntityFrameworkCore;
using IssueTracker.Infrastructure.Context;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MiniUser>> GetByName(string userName, int amount)
    {
        var users = await _context.Users
            .Where(u => u.NormalizedUserName.StartsWith(userName.ToUpper()))
            .OrderBy(u => u.NormalizedUserName)
            .Take(amount)
            .Select(x => new MiniUser
            {
                Id = x.Id,
                Username = x.UserName
            })
            .ToListAsync();
        return users;
    }

    public async Task<ApplicationUser> GetById(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user;
    }
}