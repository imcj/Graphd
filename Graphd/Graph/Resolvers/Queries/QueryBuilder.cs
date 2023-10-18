using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Graph.Resolvers.Queries;

internal class QueryBuilder
{
    public IQueryable Build(IResolveFieldContext context, IQueryable queryable)
    {
        var recursion = new Recursion(context);
        var fields = recursion.ToList();

        
        return queryable;
    }
}
