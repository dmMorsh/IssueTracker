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
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getById")]
    public async Task<IActionResult> GetById([FromQuery] Guid userId)
    {
        var user = await _userService.GetById(userId);
        return Ok(user);
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> GetByName([FromQuery] string userName, int amount = 10)
    {
        var user = await _userService.GetByName(userName, amount);
        return Ok(user);
    }
}