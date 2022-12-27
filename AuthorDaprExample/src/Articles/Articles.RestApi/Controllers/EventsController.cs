using Articles.RestApi.Domain;
using Articles.RestApi.Persistence;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace Articles.RestApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly ILogger<EventsController> logger;

    public EventsController(AppDbContext context, ILogger<EventsController> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> NewUser(CloudEvent<NewUserEvent> body)
    {
        logger.LogInformation("Received new user event: {Body}", body.ToString());
        if (string.IsNullOrEmpty(body.ToString())) return Ok();

        NewUserEvent? @event = body.Data;

        if (@event is null) return Ok();

        var userInfo = new UserInfo
        {
            UserId = @event.UserId,
            Username = @event.Username,
        };

        await context.UserInfo.AddAsync(userInfo);
        try
        {
            await context.SaveChangesAsync();

            logger.LogInformation("Saved new user info (UserId: {UserId})", @event.UserId.ToString());

            return Ok();
        } catch (Exception exception)
        {
            logger.LogError(exception, "Could not save user with Id {UserId}", @event.UserId.ToString());
            return BadRequest();
        }
    }
}

public class NewUserEvent
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}