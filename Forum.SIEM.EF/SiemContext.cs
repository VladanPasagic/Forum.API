using Forum.SIEM.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum.SIEM.EF;


public class SiemContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SiemContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public virtual DbSet<LogEntry> LogEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("MySQL_SIEM")!);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
