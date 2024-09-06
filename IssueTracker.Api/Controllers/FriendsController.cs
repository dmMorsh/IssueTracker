using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using IssueTracker.Dal.Services;
using IssueTracker.Dal.Models;
using IssueTracker.Hubs;

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
    private readonly FriendsService _friendsService;
    private readonly FriendsSubscriptionsService _subscriptionsService;
    private readonly IHubContext<ChatHub> _hubContext;

    public FriendsController(
        FriendsService friendsService,
        FriendsSubscriptionsService subscriptionsService, 
        IHubContext<ChatHub> hubContext    
        )
    {
        _friendsService = friendsService;
        _subscriptionsService = subscriptionsService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid userGuid)
    {
        IEnumerable<MiniUser> friends = await _friendsService.GetAll(userGuid);
        return Ok(friends);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid userGuid, [FromBody] IID friendid )
    {
        var success = await _friendsService.Add(userGuid, new Guid(friendid.id));
        if (success)
        return Ok("OK");
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
        _hubContext.Clients.User(friendid.id).SendAsync("ReceiveNotification", "you have new subscriber");

        var success = await _subscriptionsService.Subscribe(userGuid, new Guid(friendid.id));
        if (success)
        return Ok("OK");
        return BadRequest(ModelState);
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(Guid userGuid, [FromBody] IID friendid)
    {
        var success = await _subscriptionsService.Unsubscribe(userGuid, new Guid(friendid.id));
        if (success)
        return Ok("OK");
        return BadRequest(ModelState);
    }
}