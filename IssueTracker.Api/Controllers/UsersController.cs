using AutoMapper;
// using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;

namespace IssueTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    public readonly UsersService _usersService;

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
        var user = await _usersService.getById(userId);
        return Ok(user);
    }

    [HttpGet("getByName")]
    public async Task<IActionResult> getByName([FromQuery] string userName, int amount = 10)
    {
        var user = await _usersService.findByName(userName, amount);
        return Ok(user);
    }
}