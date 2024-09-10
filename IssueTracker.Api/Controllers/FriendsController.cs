using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Models;
using IssueTracker.Api.Hubs;

namespace IssueTracker.Controllers;

public class IID
{
    public string id { get; set; }
}

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ServiceFilter(typeof(UserGuidFilter))]
[ApiController]
[Route("api/[controller]")]
public class FriendsController : ControllerBase
{
    private readonly FriendService _friendService;
    private readonly FriendSubscriptionService _subscriptionsService;
    private readonly IHubContext<ChatHub> _hubContext;

    public FriendsController(
        FriendService friendService,
        FriendSubscriptionService subscriptionsService,
        IHubContext<ChatHub> hubContext
        )
    {
        _friendService = friendService;
        _subscriptionsService = subscriptionsService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid userGuid)
    {
        IEnumerable<MiniUser> friends = await _friendService.GetAllByUserId(userGuid);
        return Ok(friends);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid userGuid, [FromBody] IID friendid)
    {
        var success = await _friendService.AddAsync(userGuid, new Guid(friendid.id));
        if (success)
            return Ok("OK");
        else
            return BadRequest(ModelState);
    }

    [HttpGet("friendRequests")]
    public async Task<IActionResult> GetFriendRequests(Guid userGuid)
    {
        var friendRequests = await _subscriptionsService.GetFriendRequests(userGuid);
        return Ok(friendRequests);
    }

    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions(Guid userGuid)
    {
        var subscriptions = await _subscriptionsService.GetSubscriptions(userGuid);
        return Ok(subscriptions);
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(Guid userGuid, [FromBody] IID friendid)
    {
        var success = await _subscriptionsService.Subscribe(userGuid, new Guid(friendid.id));
        if (success)
        {
            _hubContext.Clients.User(friendid.id).SendAsync("ReceiveNotification", "you have new subscriber");
            return Ok("OK");
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(Guid userGuid, [FromBody] IID friendid)
    {
        var success = await _subscriptionsService.Unsubscribe(userGuid, new Guid(friendid.id));
        if (success)
            return Ok("OK");
        else
            return BadRequest(ModelState);
    }
}