using GraphQL;
using GraphQL.Resolvers;
using Graphd.Graph.Extensions;
using Graphd.Graph.Resolvers.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Graphd;

internal class FieldResolver : IFieldResolver
{

    protected DbContext dbContext;

    public FieldResolver(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async ValueTask<object?> ResolveAsync(IResolveFieldContext context)
    {
        // TODO: 
        var property = context.FieldDefinition.Name.Capitalize();
        var r = dbContext.GetType().GetProperty(property)!.GetValue(dbContext);

        var queryable = (r as IQueryable)!;
        var i = context.InputExtensions;

        if (context.HasArgument("where"))
        {
            queryable = queryable.Where(
                context.GetArgument<string>("where")
            );
        }

        var builder = new QueryBuilder();
        queryable = builder.Build(context, queryable);

        return await queryable.ToDynamicListAsync();
    }
}
