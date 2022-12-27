using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Users.Api.Application.Events;
using Users.Api.Application.Queries;
using Users.Api.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
if (builder.Configuration.GetValue<bool>("UseDapr"))
{
    builder.Configuration.AddDaprSecretStore(source =>
    {
        DaprClient daprClient = new DaprClientBuilder().Build();
        var secretDescriptors = new List<DaprSecretDescriptor>
        {
            new("ConnectionStrings:Postgres")
        };
        source.Client = daprClient;
        source.SecretDescriptors = secretDescriptors;
        source.Store = "secrets";
    });
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserInfoQuery>()
    .AddScoped<UserCreationEvent>()
    .AddDaprClient();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("Postgres") 
        ?? throw new InvalidOperationException("Missing connection string");
    options.UseNpgsql(connectionString, npgslOptions =>
    {
        npgslOptions.MigrationsHistoryTable("__EFMigrationsHistory", "users");
    });
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using IServiceScope provider = app.Services.CreateScope();
var context = provider.ServiceProvider.GetRequiredService<AppDbContext>();
var logger = provider.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    logger.LogInformation("Database migrated");
} catch (Exception e)
{
    logger.LogError(e, "Could not connect to database");
    throw;
}

app.Run();