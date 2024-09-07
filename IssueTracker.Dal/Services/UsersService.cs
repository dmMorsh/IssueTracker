using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Context;
using IssueTracker.Dal.Models;

namespace IssueTracker.Dal.Services;

public class UsersService
{
    private readonly ApplicationDbContext _context;

    public UsersService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MiniUser>> GetAll()
    {
        var users = await _context.Users
            .Select(x => new MiniUser
            {
                Id = x.Id,
                Username = x.UserName
            })
            .OrderBy(item => item.Id)
            .ToListAsync();
        return users;
    }

    public async Task<IEnumerable<MiniUser>> FindByName(string userName, int amount)
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

    public async Task<ApplicationUser> FindById(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<ApplicationUser> FindById(string userId)
    {
        return await FindById(new Guid(userId));
    }
}