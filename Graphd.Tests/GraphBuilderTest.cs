using GraphQL;
using GraphQL.Execution;
using GraphQL.Types;
using Graphd.Graph.Builder;
using Graphd.Graph.Proxies;
using Graphd.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using Xunit;

namespace Graphd.Tests;

public class GraphBuilderTest
{
    [Fact]
    public async Task TestDiscovery()
    {
        var dbContext = new GraphDbContext(new DbContextOptionsBuilder().UseInMemoryDatabase("test").Options);
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

        var includeMethod = IncludableQueryableProxy.includeMethod;

        //dbContext.Heros.Include("")

        var query = GraphTypeBuilder.Discovery(dbContext);

        var variables = new Dictionary<string, object?>() { { "parameters", new object[] { new string[] { "R2-D2", "R2-D3" } } } };
        var inputs = new Inputs(variables);

        var result = await new DocumentExecuter().ExecuteAsync(options =>
        {
            options.Query = "{ heros(where: \"@0.Contains(Name)\") { id, name, factory { id, name, droids { id, name } }, tags(orderBy: \"id desc\") { id, name } } }";
            options.OperationName = string.Empty;
            options.Schema = new Schema()
            {
                Query = query,
            };
            options.Variables = inputs;
            options.Extensions = inputs;
        });

        var r = GraphSerializer.Serialize(result.Data as RootExecutionNode); 
        Assert.Null(result.Errors);
    }
}