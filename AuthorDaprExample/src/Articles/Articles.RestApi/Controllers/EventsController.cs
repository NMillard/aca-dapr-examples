using System.Text.Json;
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

        NewUserEvent? e2 = body.Data;

        if (e2 is null) return Ok();

        var userInfo = new UserInfo
        {
            UserId = e2.UserId,
            Username = e2.Username,
        };

        await context.UserInfo.AddAsync(userInfo);
        try
        {
            await context.SaveChangesAsync();

            logger.LogInformation("Saved new user info (UserId: {UserId})", e2.UserId.ToString());

            return Ok();
        } catch (Exception exception)
        {
            logger.LogError(exception, "Could not save user with Id {UserId}", e2.UserId.ToString());
            return BadRequest();
        }
    }
}

public class CloudEventWrapper<T>
{
    public string Data { get; set; }

    public T? ConvertToType()
    {
        return JsonSerializer.Deserialize<T>(Data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}

public class NewUserEvent
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
}