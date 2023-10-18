using Graphd.HTTPServer;
using GraphQL;
using GraphQL.SystemTextJson;
using Graphd.Graph.Builder;
using Graphd.Graph.Types;
using Graphd.Tests.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddDbContext<GraphDbContext>(options =>
    {
        options.UseInMemoryDatabase("demo");
    })
    .AddSingleton((sp) =>
    {
        var dbContext = sp.GetService<GraphDbContext>()!;
        return (SchemaQueryGraphType)GraphTypeBuilder.Discovery(dbContext);
    })
    .AddSingleton<IGraphQLSerializer, GraphQLSerializer>()
    .AddHostedService<GraphdHostedService>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
