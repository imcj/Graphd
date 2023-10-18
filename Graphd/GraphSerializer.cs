using GraphQL.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graphd;

public class GraphSerializer
{
    public static string Serialize(RootExecutionNode node)
    {
        var dict = node
            .SubFields
            ?.Select(child => new { Key = child.Name, Value = child.Result})
            ?.ToDictionary(x => x.Key, x => x.Value);

        return JsonSerializer.Serialize(dict, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        });
    }
}
