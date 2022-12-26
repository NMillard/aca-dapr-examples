using Microsoft.EntityFrameworkCore;
using Users.Api.Persistence;
using Users.Domain;

namespace Users.Api.Application.Events;

public class UserCreationEvent
{
    private readonly AppDbContext context;

    public UserCreationEvent(AppDbContext context) => this.context = context;

    public async Task<Guid?> ExecuteAsync(Username username)
    {
        var user = new User { Username = username };
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        } catch (Exception ex) when (ex is DbUpdateException or DbUpdateConcurrencyException)
        {
            Console.WriteLine(ex);
            throw;
        }
        return user.Id;
    }
}