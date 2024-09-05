using Microsoft.AspNetCore.SignalR;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;

namespace IssueTracker.Hubs;
public class ChatHub : Hub
{
    public readonly MessagesService _messagesService;

    public ChatHub(MessagesService messagesService)
    {
        _messagesService = messagesService;
    }

    public async Task SendMessage(UserMessage m)
    {
        m.dateSent = DateTime.UtcNow;

        UserMessage returnMessage = new UserMessage
        {
            chatId = m.chatId,
            sender = m.sender,
            message = m.message,
            dateSent = m.dateSent
        };

        var success = await _messagesService.Add(returnMessage);
        if (success)
        await Clients.Group(m.chatId.ToString()).SendAsync("ReceiveMessage", returnMessage);
    }

    public override async Task OnConnectedAsync()
    {
        // Получить идентификатор чата из контекста подключения, предположим, что он передается как параметр в URL запроса
        var chatId = Context.GetHttpContext()!.Request.Query["chatId"];

        // Проверить, что идентификатор чата не пустой
        if (!string.IsNullOrEmpty(chatId))
        {
            // Присоединить пользователя к группе чата
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        await base.OnConnectedAsync();
    }

}

