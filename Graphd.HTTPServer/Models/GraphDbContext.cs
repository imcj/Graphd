using Microsoft.EntityFrameworkCore;

namespace Graphd.Tests.Models;

public class GraphDbContext : DbContext
{
    public DbSet<Droid> Heros { get; set; }

    public DbSet<Factory> Factories { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public GraphDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }
}
