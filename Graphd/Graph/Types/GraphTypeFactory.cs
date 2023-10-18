using GraphQL.Types;
using Graphd.Graph.Extensions;

namespace Graphd.Graph.Types;

public class GraphTypeFactory
{

    protected GraphTypeCollection graphTypes = new();

    protected List<Type> entityTypes;

    public GraphTypeFactory(ICollection<Type>? entityTypes = null)
    {
        this.entityTypes = entityTypes?.ToList() ?? new List<Type>();
    }


    public bool IsEntityType(Type type)
    {
        return entityTypes.Any(t => t == type);
    }

    public Type MakeGraphType(Type type)
    {
        var key = $"Graph<{type.Name}>";


        if (graphTypes.Contains(key))
        {
            return graphTypes.Get(key);
        }

        Type entityGraphType;

        if (type.IsRelationship())
        {
            entityGraphType = MakeListGraphType(
                type
                    .GetGenericArguments()
                    .First()
            );
        }
        else if (IsEntityType(type))
        {
            entityGraphType = typeof(EntityObjectGraphType<>).MakeGenericType(type);
        }
        else
        {
            entityGraphType = type;
        }


        graphTypes.Set(key, entityGraphType);
        return entityGraphType;
    }

    public Type MakeListGraphType(Type entityType)
    {
        var key = $"List<{entityType.Name}>";
        if (graphTypes.Contains(key))
        {
            return graphTypes.Get(key);
        }

        var entityGraphType = MakeGraphType(entityType);
        var listGraphType = typeof(ListGraphType<>).MakeGenericType(entityGraphType);

        graphTypes.Set(key, listGraphType);
        return listGraphType;
    }
}
