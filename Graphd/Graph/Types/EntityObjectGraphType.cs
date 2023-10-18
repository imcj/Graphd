using GraphQL.Types;
using Graphd.Graph.Builder;

namespace Graphd.Graph.Types;

public class EntityObjectGraphType<T> : ObjectGraphType<T>
{
    public EntityObjectGraphType() : base()
    {
        new GraphTypeBuilder(this, typeof(T)).Build();
    }

    public EntityObjectGraphType(bool buildProperty) : base()
    {

    }
}
