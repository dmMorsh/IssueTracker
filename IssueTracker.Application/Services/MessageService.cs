using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class MessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task<IEnumerable<UserMessage>> GetAllByChatIdAsync(int chatId)
    {
        return _messageRepository.GetAllByChatIdAsync(chatId);
    }

    public async Task<bool> AddAsync(UserMessage message)
    {
        await _messageRepository.AddAsync(message);
        return true;
    }

    public async Task<bool> UpdateAsync(UserMessage message)
    {
        await _messageRepository.UpdateAsync(message);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _messageRepository.DeleteAsync(id);
        return true;
    }
}