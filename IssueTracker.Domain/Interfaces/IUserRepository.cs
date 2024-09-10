using IssueTracker.Domain.Models;

namespace IssueTracker.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<MiniUser>> GetByName(string userName, int amount);
    Task<ApplicationUser> GetById(Guid userId);
}