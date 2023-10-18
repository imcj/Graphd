using GraphQL.Resolvers;
using Graphd.Graph.Types;
using Microsoft.EntityFrameworkCore;

namespace Graphd.Graph.Builder;

internal class EntityFrameworkSchemaQueryGraphTypeBuilder : TypeSchemaQueryGraphTypeBuilder
{
    public EntityFrameworkSchemaQueryGraphTypeBuilder(DbContext dbContext, GraphTypeFactory factory, IFieldResolver resolver) : base(GetEntities(dbContext), factory, resolver)
    {
    }

    public static IDictionary<string, Type> GetEntities(object dbContext)
    {
        var returns = new Dictionary<string, Type>();

        var properties = dbContext
            .GetType()
            .GetProperties();

        var sets = properties
            .Where(property => property.PropertyType.Name.StartsWith("DbSet"));

        foreach (var set in sets)
        {
            returns[set.Name] = set.PropertyType.GetGenericArguments().First();
        }

        return returns;
    }
}
