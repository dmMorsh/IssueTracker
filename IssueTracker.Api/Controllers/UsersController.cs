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
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _usersService.GetAll();
        return Ok(items);
    }

    [HttpGet("getById")]
    public async Task<IActionResult> getById([FromQuery] Guid userId)
    {
        var user = await _usersService.FindById(userId);
        return Ok(user);
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> getByName([FromQuery] string userName, int amount = 10)
    {
        var user = await _usersService.FindByName(userName, amount);
        return Ok(user);
    }
}