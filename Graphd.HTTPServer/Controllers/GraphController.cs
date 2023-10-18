using GraphQL.Execution;
using GraphQL;
using GraphQL.SystemTextJson;
using Graphd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Types;
using Graphd.Graph.Builder;
using Microsoft.EntityFrameworkCore;
using Graphd.Tests.Models;
using System.Text;
using Graphd.Graph.Types;

namespace Graphd.HTTPServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GraphController : ControllerBase
{
    protected GraphDbContext dbContext;

    protected SchemaQueryGraphType schemaQueryGraphType;

    public GraphController(GraphDbContext dbContext, SchemaQueryGraphType schemaQueryGraphType)
    {
        this.dbContext = dbContext;
        this.schemaQueryGraphType = schemaQueryGraphType;
    }

    [HttpPost]
    public async Task<IActionResult> Query()
    {
        var serializer = new GraphQLSerializer(b =>
        {
            b.PropertyNameCaseInsensitive = true;
        });

        var command = await serializer.ReadAsync<GraphQLCommand>(Request.Body)!;

        var schema = new Schema()
        {
            Query = schemaQueryGraphType,
        };

        schema.EnableExperimentalIntrospectionFeatures();
        var result = await new DocumentExecuter().ExecuteAsync(options =>
        {
            options.Query = command.Query;
            //options.OperationName = string.Empty;
            options.Schema = schema;
            options.Variables = command.Variables;
            //options.Extensions = inputs;
        });


        

        Response.Headers.ContentType = "application/json";
        await serializer.WriteAsync(Response.Body, result);


        return new EmptyResult();
    }
}
