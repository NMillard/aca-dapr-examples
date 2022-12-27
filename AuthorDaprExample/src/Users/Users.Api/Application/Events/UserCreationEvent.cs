using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using Users.Api.Persistence;
using Users.Domain;

namespace Users.Api.Application.Events;

public class UserCreationEvent
{
    private readonly AppDbContext context;
    private readonly DaprClient client;

    public UserCreationEvent(AppDbContext context, DaprClient client)
    {
        this.context = context;
        this.client = client;
    }

    public async Task<Guid?> ExecuteAsync(Username username)
    {
        var user = new User { Username = username };
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await client.PublishEventAsync("pubsub", "userevents", new
            {
                UserId = user.Id,
                Username = user.Username.Value
            });
        } catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
        {
            Console.WriteLine(ex);
            throw;
        }
        
        return user.Id;
    }
}