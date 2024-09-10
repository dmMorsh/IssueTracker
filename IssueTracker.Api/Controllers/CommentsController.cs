using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;
using IssueTracker.Api.DTOs;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly CommentService _commentService;

    public CommentsController(
        IMapper mapper,
        CommentService commentService
        )
    {
        _mapper = mapper;
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketCommentDto commentDto)
    {
        var comment = _mapper.Map<TicketComment>(commentDto);
        var commentResult = await _commentService.AddAsync(comment);
        if (commentResult == null)
            return BadRequest(ModelState);

        var commentDtoResult = _mapper.Map<TicketCommentDto>(commentDto);
        return Ok(commentDtoResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketCommentDto commentDto)
    {
        var comment = _mapper.Map<TicketComment>(commentDto);
        var success = await _commentService.UpdateAsync(id, comment);
        if (success)
            return Ok();
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _commentService.DeleteAsync(id);
        if (success)
            return Ok();
        else
            return BadRequest(new { message = "bad request" });
    }
}