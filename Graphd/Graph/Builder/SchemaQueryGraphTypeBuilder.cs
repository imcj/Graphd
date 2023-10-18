using GraphQL.Resolvers;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Graphd.Graph.Types;
using Graphd.NameStrategy;

namespace Graphd.Graph.Builder;

internal class SchemaQueryGraphTypeBuilder
{

    protected INameStrategy nameStrategy;

    protected IFieldResolver resolver;

    protected GraphTypeFactory factory;


    public SchemaQueryGraphTypeBuilder(GraphTypeFactory factory, INameStrategy nameStrategy, IFieldResolver resolver)
    {
        this.nameStrategy = nameStrategy;
        this.resolver = resolver;
        this.factory = factory;
    }

    public SchemaQueryGraphTypeBuilder(GraphTypeFactory factory, IFieldResolver resolver)
    {
        this.factory = factory;
        this.resolver = resolver;

        nameStrategy = new CamcelCaseNameStrategy();
    }


    public FieldType CreateFieldType(string name, Type graphType)
    {
        return new FieldType
        {
            Name = name,
            Type = graphType,
            Resolver = resolver,
            Arguments = new QueryArguments(
                new QueryArgument<StringGraphType> { Name = "where" },
                new QueryArgument<AnyScalarGraphType> { Name = "orderBy" }
            )
        };
    }
}
