using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;
using Persistence.Entities;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    private PersistenceOptions _persistenceOptions;
    public DbSet<Story> Stories { get; set; }
    public DbSet<StoryNode> Nodes { get; set; }
    public DbSet<StoryOption> Options { get; set; }
    private readonly ILogger<ApplicationDbContext> _logger;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<PersistenceOptions> persistenceOptions, ILogger<ApplicationDbContext> logger) : base(options)
    {
        _logger = logger;
        _persistenceOptions = persistenceOptions.Value;

        this.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _persistenceOptions.ConnectionString;
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<StoryNode>()
            .HasMany<StoryOption>()
            .WithOne(e => e.NextNode)
            .HasForeignKey(e => e.NextNodeId);
    }

}