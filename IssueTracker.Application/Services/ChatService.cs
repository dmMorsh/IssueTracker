using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class ChatService
{
    private readonly IChatRepository _chatRepository;
    
    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public Task<IEnumerable<object>> GetByUserId(Guid userId)
    {
        return _chatRepository.GetByUserId(userId);
    }

    public Task<int?> GetPersonalChatId(Guid userId, Guid secondUserId)
    {
        return _chatRepository.GetPersonalChatId(userId, secondUserId);
    }

    public Task<IEnumerable<MiniUser>> GetGuysByChatId(int chatId)
    {
        return _chatRepository.GetGuysByChatId(chatId);
    }

    public async Task<bool> AddAsync(Chat Chat)
    {
        await  _chatRepository.AddAsync(Chat);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _chatRepository.DeleteAsync(id);
        return true;
    }
}