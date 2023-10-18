using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Graphd.Graph.Extensions;
using Graphd.Graph.Types;
using Microsoft.EntityFrameworkCore;

namespace Graphd.Graph.Builder;

public class GraphTypeBuilder
{
    public Type EntityType { get; set; }

    protected IObjectGraphType instance;

    static GraphTypeFactory graphTypeFactory;

    static int INDEX = 2;

    public GraphTypeBuilder(dynamic instance, Type entityType)
    {
        this.instance = instance;
        instance.Name = "EntityObjectGraphType_" + entityType.Namespace?.Replace(".", "_") + "_" + entityType.Name;
        EntityType = entityType;
    }

    public static IObjectGraphType Discovery(DbContext dbContext)
    {
        var sets = EntityFrameworkSchemaQueryGraphTypeBuilder.GetEntities(dbContext);
        graphTypeFactory = new GraphTypeFactory(sets.Values);
        var resolver = new FieldResolver(dbContext);

        var builder = new EntityFrameworkSchemaQueryGraphTypeBuilder(dbContext, graphTypeFactory, resolver);
        var query = builder.Build();

        return query;
    }

    public void Build()
    {
        var proxy = new ObjectGraphTypeProxy(instance, EntityType);

        var properties = EntityType.GetProperties().Select(property => property.Name);
        foreach (var property in EntityType.GetProperties())
        {
            var fieldType = property.PropertyType;
            if (graphTypeFactory.IsEntityType(fieldType) || fieldType.IsRelationship())
            {
                fieldType = graphTypeFactory.MakeGraphType(fieldType);
                //proxy.AddFieldByName(property.Name, fieldType);

                instance.AddField(new FieldType()
                {
                    Name = property.Name,
                    Type = fieldType,
                    Arguments = new QueryArguments(
                        new QueryArgument<AnyScalarGraphType>() {  Name = "orderBy" }
                    )
                });
            }
            else
            {
                proxy.FieldByLambda(property.Name, fieldType);
            }
        }
    }
}