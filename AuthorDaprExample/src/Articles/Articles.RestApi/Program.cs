using System.Reflection;
using Articles.RestApi.Persistence;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
if (builder.Configuration.GetValue<bool>("UseDapr"))
{
    // Inquire dapr about the secret store "secrets" 
    // and load the key/value ConnectionStrings:Postgres
    // Note: the secrets are coming from the file "secrets.json" in .dapr/components
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
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    string? connectionString = builder.Configuration.GetConnectionString("Postgres");
#if  DEBUG
    // Don't log the connection string in production. Logging it here is useful when learning
    // how dapr works and where it loads the connection string from.
    var logger = provider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Using connection string {ConnectionString}", connectionString);
#endif
    
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "articles");
    });
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Articles REST API";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Articles REST API");
    options.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

// Don't do this in production.
// Read the readme.md section on database migrations.
using IServiceScope provider = app.Services.CreateScope();
var context = provider.ServiceProvider.GetRequiredService<AppDbContext>();
var logger = provider.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
} catch (Exception e)
{
    logger.LogError(e, "Could not connect to database");
    throw;
}

app.Run();