using Graphd.Tests.Models;

namespace Graphd.HTTPServer.Extensions;

public static class GraphDbContextExtensions
{
    public static async Task Initialize(this GraphDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.AddRange(
            new Droid()
            {
                Id = "1",
                Name = "R2-D2",
                Factory = new()
                {
                    Id = "1",
                    Name = "R2",
                    Droids = new List<Droid>() { new Droid() { Id = "4", Name = "R2-D4" } }
                },
                Tags = new()
                {
                    new Tag() { Id = 1, Name = "R2" },
                    new Tag() { Id = 2, Name = "D2" },
                }
            },
            new Droid()
            {
                Id = "2",
                Name = "R2-D3",
            }
        );

        await dbContext.SaveChangesAsync();
    }
}
