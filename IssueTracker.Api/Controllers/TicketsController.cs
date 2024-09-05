using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;
using AutoMapper;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ServiceFilter(typeof(UserGuidFilter))]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly TicketsService _ticketsService;
    private readonly UsersService _usersService;
    
    public TicketsController(
        TicketsService ticketsService
        , UsersService usersService
        , IMapper mapper)
    {
        _mapper = mapper;
        _ticketsService = ticketsService;
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _ticketsService.GetAll();
        var ticketsDto = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDto>>(items);
        return Ok(ticketsDto);
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tuple = await _ticketsService.GetPage(page, pageSize);   
        var _items = tuple.Item1;
        int totalCount = tuple.Item2;
        int _totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (_items.Count() == 0)
            _totalPages = 1;
        
        return Ok(new
        {
            totalPages = _totalPages,
            tickets = _items
        });
    }

    [HttpGet("watching/page")]
    public async Task<IActionResult> GetWatchingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tuple = await _ticketsService.GetWatchingPage(userGuid, page, pageSize);
        var _items = tuple.Item1;
        int totalCount = tuple.Item2;
        int _totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (_items.Count() == 0)
            _totalPages = 1;

        return Ok(new
        {
            totalPages = _totalPages,
            tickets = _items
        });
    }

    [HttpGet("executing/page")]
    public async Task<IActionResult> GetExecutingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tuple = await _ticketsService.GetExecutingPage(userGuid, page, pageSize);
        var _items = tuple.Item1;
        int totalCount = tuple.Item2;
        int _totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (_items.Count() == 0)
            _totalPages = 1;

        return Ok(new
        {
            totalPages = _totalPages,
            tickets = _items
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
    {
        var _itemDto = await _ticketsService.Add(ticketDto);
        return Ok(_itemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
    {
        var success = await _ticketsService.Update(id, ticketDto);
        if (success)
            return Ok();
        else
            return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _ticketsService.Remove(id);
        if (success)          
            return Ok();
        else
            return BadRequest(new {message = "not found"});
    }

}