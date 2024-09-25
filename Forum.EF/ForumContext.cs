using Forum.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum.EF;

public class ForumContext : IdentityDbContext<User>
{
    private readonly IConfiguration _configuration;

    public ForumContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("MySQL")!);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
