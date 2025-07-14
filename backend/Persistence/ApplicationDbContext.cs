using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    private PersistenceOptions _persistenceOptions;
    public DbSet<Story> Stories { get; set; }
    public DbSet<StoryNode> Nodes { get; set; }
    public DbSet<StoryOption> Options { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<PersistenceOptions> persistenceOptions) : base(options)
    {
        _persistenceOptions = persistenceOptions.Value;
        this.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: Add Configuration Helper instead
        var connectionString = _persistenceOptions.ConnectionString;
        // optionsBuilder.UseNpgsql
    }

}