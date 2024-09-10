using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<MiniUser>> GetByName(string userName, int amount)
    {
        return _userRepository.GetByName(userName, amount);
    }

    public Task<ApplicationUser> GetById(Guid userId)
    {
        return _userRepository.GetById(userId);
    }

    public Task<ApplicationUser> GetById(string userId)
    {
        return GetById(new Guid(userId));
    }
}