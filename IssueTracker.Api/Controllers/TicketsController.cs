using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;
using IssueTracker.Hubs;
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
    private readonly IHubContext<ChatHub> _hubContext;

    public TicketsController(
        TicketsService ticketsService,
        IMapper mapper,
        IHubContext<ChatHub> hubContext
        )
    {
        _mapper = mapper;
        _ticketsService = ticketsService;
        _hubContext = hubContext;
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
        var items = tuple.Item1;
        int totalCount = tuple.Item2;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (items.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = items
        });
    }

    [HttpGet("watching/page")]
    public async Task<IActionResult> GetWatchingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tuple = await _ticketsService.GetWatchingPage(userGuid, page, pageSize);
        var items = tuple.Item1;
        int totalCount = tuple.Item2;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (items.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = items
        });
    }

    [HttpGet("executing/page")]
    public async Task<IActionResult> GetExecutingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tuple = await _ticketsService.GetExecutingPage(userGuid, page, pageSize);
        var items = tuple.Item1;
        int totalCount = tuple.Item2;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (items.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = items
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
    {
        TicketDto itemDto = await _ticketsService.Add(ticketDto);
        if(itemDto.ExecutorId != "")
            _hubContext.Clients.User(itemDto.ExecutorId).SendAsync("ReceiveNotification", "you have new ticket");

        return Ok(itemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
    {
        var tuple = await _ticketsService.Update(id, ticketDto);
        var success = tuple.Item1;
        if (success)
        {
            string executorId = tuple.Item2;
            if (executorId != "")
                _hubContext.Clients.User(executorId).SendAsync("ReceiveNotification", "you have new ticket");

            return Ok();
        }   
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _ticketsService.Remove(id);
        if (success)
            return Ok();
        else
            return BadRequest(new { message = "not found" });
    }
}