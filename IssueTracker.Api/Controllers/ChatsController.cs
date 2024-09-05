using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ServiceFilter(typeof(UserGuidFilter))]
[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ChatsService _chatsService;
    public ChatsController(ChatsService chatsService)
    {
        _chatsService = chatsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid userGuid)
    {
        var items = await _chatsService.Get(userGuid);
        return Ok(items);
    }

    [HttpGet("getChatId")]
    public async Task<IActionResult> getChatId(Guid userGuid, [FromQuery] Guid userId)
    {
        var chatId = await _chatsService.getPersonalChatId(userGuid, userId);
        if (chatId != null)
            return Ok(chatId);
        else
            return BadRequest(new {message = "bad user"});
    }

    [HttpGet("getGuysById")]
    public async Task<IActionResult> getGuysById([FromQuery] int id)
    {
        var guys = await _chatsService.getGuysById(id);
        if (guys == null) return BadRequest(new {message = "Something went wrong"});
        return Ok(guys);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ChatEntity item)
    {
        var success = await _chatsService.Add(item);
        if (success)
            return Ok(item);
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _chatsService.Remove(id);
        if (success)
            return Ok();
        else
            return BadRequest(new {message = "bad user"});
    }

}