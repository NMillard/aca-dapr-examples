using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Application.Events;
using Users.Api.Application.Queries;
using Users.Domain;

namespace Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserInfoQuery query;
    private readonly UserCreationEvent creationEvent;

    public UsersController(UserInfoQuery query, UserCreationEvent creationEvent)
    {
        this.query = query;
        this.creationEvent = creationEvent;
    }

    [HttpGet("{id:guid}/Info")]
    public async Task<ActionResult<UserInfoResponse>> GetInfo(Guid id)
    {
        UserInfo? info = await query.ExecuteAsync(id);

        return info is not null
            ? Ok(new UserInfoResponse(info.username))
            : NotFound();
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        Guid? e = await creationEvent.ExecuteAsync(new Username
        {
            Value = request.Username
        });
        return Ok();
    }
}

public record CreateUserRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Username { get; set; }
};

public record UserInfoResponse(string Username);