using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessagesController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("getById")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var messages = await _messageService.GetAllByChatIdAsync(id);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserMessage item)
    {
        var success = await _messageService.AddAsync(item);
        if (success)
            return Ok(item);
        else
            return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserMessage item)
    {
        var success = await _messageService.UpdateAsync(item);
        if (success)
            return Ok();
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _messageService.DeleteAsync(id);
        if (success)
            return Ok();
        else
            return BadRequest(new { message = "bad request" });
    }
}