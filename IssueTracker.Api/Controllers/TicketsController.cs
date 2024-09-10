using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;
using IssueTracker.Api.Hubs;
using IssueTracker.Api.DTOs;

namespace IssueTracker.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ServiceFilter(typeof(UserGuidFilter))]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly TicketService _ticketService;
    private readonly IHubContext<ChatHub> _hubContext;

    public TicketsController(
        IMapper mapper,
        TicketService ticketService,
        IHubContext<ChatHub> hubContext
        )
    {
        _mapper = mapper;
        _ticketService = ticketService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _ticketService.GetAllAsync();
        var ticketsDto = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDto>>(items);
        return Ok(ticketsDto);
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tickets = await _ticketService.GetPage(page, pageSize);
        var ticketsDto = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDto>>(tickets);

        int totalCount = await _ticketService.GetPageCount();
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (tickets.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = ticketsDto
        });
    }

    [HttpGet("watching/page")]
    public async Task<IActionResult> GetWatchingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tickets = await _ticketService.GetWatchingPage(userGuid, page, pageSize);
        var ticketsDto = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDto>>(tickets);

        int totalCount = await _ticketService.GetWatchingPageCount(userGuid);
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (tickets.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = ticketsDto
        });
    }

    [HttpGet("executing/page")]
    public async Task<IActionResult> GetExecutingPage(Guid userGuid, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tickets = await _ticketService.GetExecutingPage(userGuid, page, pageSize);
        var ticketsDto = _mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketDto>>(tickets);

        int totalCount = await _ticketService.GetExecutingPageCount(userGuid);
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        if (tickets.Count() == 0)
            totalPages = 1;

        return Ok(new
        {
            totalPages = totalPages,
            tickets = ticketsDto
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
    {
        var ticket = _mapper.Map<TicketDto, Ticket>(ticketDto);
        var ticketResult = await _ticketService.AddAsync(ticket);
        if(!ticket.ExecutorId.Equals(Guid.Empty))
            _hubContext.Clients.User(ticket.ExecutorId.ToString()).SendAsync("ReceiveNotification", "you have new ticket");

        var ticketDtoResult = _mapper.Map<TicketDto>(ticket);
        return Ok(ticketDtoResult);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
    {
        var oldExecutorId = await _ticketService.GetExecutorIdByTicketId(id);
        var ticket = _mapper.Map<TicketDto, Ticket>(ticketDto);
        var success = await _ticketService.UpdateAsync(id, ticket);
        if (success)
        {
            if (ticket.ExecutorId != null && !ticket.ExecutorId.Equals(Guid.Empty) && !ticket.ExecutorId.Equals(oldExecutorId))
                _hubContext.Clients.User(ticket.ExecutorId.ToString()).SendAsync("ReceiveNotification", "you have new ticket");

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
        var success = await _ticketService.DeleteAsync(id);
        if (success)
            return Ok();
        else
            return BadRequest(new { message = "not found" });
    }
}