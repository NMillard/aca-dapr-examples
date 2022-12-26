using Microsoft.EntityFrameworkCore;
using Users.Api.Persistence;
using Users.Domain;

namespace Users.Api.Application.Queries;

public class UserInfoQuery
{
    private readonly AppDbContext context;

    public UserInfoQuery(AppDbContext context) => this.context = context;

    public async Task<UserInfo?> ExecuteAsync(Guid userId)
    {
        User? user = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
        return user?.UserInfo ?? null;
    }
}