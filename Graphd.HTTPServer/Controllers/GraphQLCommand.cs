using GraphQL;

namespace Graphd.HTTPServer.Controllers;

public class GraphQLCommand
{
    public string Query { get; set; }

    public Inputs Variables { get; set; }
}
