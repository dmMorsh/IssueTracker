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
                id = x.Id,
                username = x.UserName
            })
            .OrderBy(item => item.id)
            .ToListAsync();

        return users;
    }

    public async Task<IEnumerable<MiniUser>> findByName(string userName, int amount)
    {
        var users = await _context.Users
            .Where(u => u.NormalizedUserName.StartsWith(userName.ToUpper()))
            .OrderBy(u => u.NormalizedUserName)
            .Take(amount)
            .Select(x => new MiniUser
            {
                id = x.Id,
                username = x.UserName
            })
            .ToListAsync();

        return users;
    }

    public async Task<ApplicationUser> getById(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }



}