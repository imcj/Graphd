using GraphQL.Resolvers;
using GraphQL.Types;
using Graphd.Graph.Types;

namespace Graphd.Graph.Builder;

internal class TypeSchemaQueryGraphTypeBuilder : SchemaQueryGraphTypeBuilder, ISchameQueryGraphTypeBuilder
{

    protected IDictionary<string, Type> Fields = new Dictionary<string, Type>();

    public TypeSchemaQueryGraphTypeBuilder(IDictionary<string, Type> sets, GraphTypeFactory factory, IFieldResolver resolver) : base(factory, resolver)
    {
        Fields = sets;
    }

    public IObjectGraphType Build()
    {
        var queryGraphType = new SchemaQueryGraphType();

        foreach (var key in Fields.Keys)
        {
            AddQueryField(queryGraphType, key);
        }

        return queryGraphType;
    }

    public void AddQueryField(IObjectGraphType queryGraphType, string key)
    {
        key = nameStrategy.Convert(key);

        var type = Fields[key];
        var graphType = typeof(ListGraphType<>).MakeGenericType(factory.MakeGraphType(type));

        queryGraphType.AddField(CreateFieldType(key, graphType));
    }
}
