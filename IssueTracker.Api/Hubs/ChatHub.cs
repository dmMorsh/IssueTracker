using Microsoft.AspNetCore.SignalR;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public readonly MessageService _messageService;

    public ChatHub(MessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessage(UserMessage m)
    {
        m.DateSent = DateTime.UtcNow;
        UserMessage returnMessage = new UserMessage
        {
            ChatId = m.ChatId,
            Sender = m.Sender,
            Message = m.Message,
            DateSent = m.DateSent
        };
        var success = await _messageService.AddAsync(returnMessage);
        if (success)
            await Clients.Group(m.ChatId.ToString()).SendAsync("ReceiveMessage", returnMessage);
    }

    public override async Task OnConnectedAsync()
    {
        var chatId = Context.GetHttpContext()!.Request.Query["chatId"];
        if (!string.IsNullOrEmpty(chatId))
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

        await base.OnConnectedAsync();
    }
}

