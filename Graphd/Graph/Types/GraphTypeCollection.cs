using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Graph.Types;

public class GraphTypeCollection
{
    protected IDictionary<string, Type> graphTypes = new Dictionary<string, Type>();

    public bool Contains(string key)
    {
        return graphTypes.ContainsKey(key);
    }

    public Type Get(string key)
    {
        return graphTypes[key];
    }

    public void Set(string key, Type value)
    {
        graphTypes[key] = value;
    }
}
