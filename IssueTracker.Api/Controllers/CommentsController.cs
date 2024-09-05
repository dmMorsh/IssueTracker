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
public class CommentsController : ControllerBase
{
    private readonly CommentsService _commentsService;

    public CommentsController(CommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ticketCommentDto = _commentsService.GetAll();
        return Ok(ticketCommentDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketCommentDto itemDto)
    {
        var _itemDto = _commentsService.Add(itemDto);
        if (_itemDto != null)
            return Ok(_itemDto);
        else
            return BadRequest(ModelState);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketCommentDto itemDto)
    {
        var success = await _commentsService.Update(id, itemDto);
        if (success)
            return Ok();
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _commentsService.Remove(id);
        if (success) 
            return Ok();
        else
            return BadRequest(new { message = "bad request" });
    }


}