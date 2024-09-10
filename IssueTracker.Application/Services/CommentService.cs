using IssueTracker.Domain.Interfaces;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Services;

public class CommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<bool> AddAsync(TicketComment comment)
    {
        await _commentRepository.AddAsync(comment);
        return true;
    }

    public async Task<bool> UpdateAsync(int id, TicketComment comment)
    {
        await _commentRepository.UpdateAsync(comment);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _commentRepository.DeleteAsync(id);
        return true;
    }
}