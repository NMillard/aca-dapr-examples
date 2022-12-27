using Articles.RestApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Articles.RestApi.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public required DbSet<Article> Articles { get; init; }
    public required DbSet<UserInfo> UserInfo { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("articles");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

internal class ArticlesConfig : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).HasMaxLength(150).IsRequired();
    }
}

public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("userinfo");
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.Created).IsRequired();
        builder.Property(u => u.LastUpdated).IsRequired(false);

        builder.HasIndex(u => u.Username);
    }
}