using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly MessagesService _messagesService;

    public MessagesController(MessagesService messagesService)
    {
        _messagesService = messagesService;
    }

    // [HttpGet]
    // public async Task<IActionResult> Get()
    // {
    //     var items = await _messagesService.Get();
    //     return Ok(items);
    // }

    [HttpGet("getById")]
    public async Task<IActionResult> getById([FromQuery] int id)
    {
        var messages = await _messagesService.getById(id);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserMessage item)
    {
        var success = await _messagesService.Add(item);
        if (success)
            return Ok(item);
        else
            return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserMessage item)
    {
        var success = await _messagesService.Update(id, item);
        if (success)
            return Ok();
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _messagesService.Remove(id);
        if (success)
            return Ok();
        else
            return BadRequest(new {message = "bad request"});
    }



}