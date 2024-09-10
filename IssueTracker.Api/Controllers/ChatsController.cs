using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ServiceFilter(typeof(UserGuidFilter))]
[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ChatService _chatService;
    public ChatsController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserId(Guid userGuid)
    {
        var items = await _chatService.GetByUserId(userGuid);
        return Ok(items);
    }

    [HttpGet("getChatId")]
    public async Task<IActionResult> GetChatId(Guid userGuid, [FromQuery] Guid userId)
    {
        var chatId = await _chatService.GetPersonalChatId(userGuid, userId);
        if (chatId != null)
            return Ok(chatId);
        else
            return BadRequest(new { message = "bad user" });
    }

    [HttpGet("getGuysById")]
    public async Task<IActionResult> GetGuysByChatId([FromQuery] int id)
    {
        var guys = await _chatService.GetGuysByChatId(id);
        if (guys != null)
            return Ok(guys);
        else
            return BadRequest(new { message = "Something went wrong" });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Chat item)
    {
        var success = await _chatService.AddAsync(item);
        if (success)
            return Ok(item);
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _chatService.DeleteAsync(id);
        if (success)
            return Ok();
        else
            return BadRequest(new { message = "bad user" });
    }
}