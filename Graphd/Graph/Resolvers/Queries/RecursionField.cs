using GraphQLParser.AST;

namespace Graphd.Graph.Resolvers.Queries;

internal class RecursionField
{
    protected string field;

    protected RecursionField? parent;

    protected bool hasChildren;

    protected Type type;

    protected GraphQLArguments? arguments;

    public bool HasChildren { get => hasChildren; }

    public RecursionField(string field, Type type, GraphQLArguments? arguments, RecursionField? parent)
    {
        this.field = field;
        this.parent = parent;
        this.arguments = arguments;
        this.type = type;
    }

    public List<RecursionField> ToList()
    {
        RecursionField p = parent;
        var fields = new List<RecursionField>();
        fields.Add(this);

        while (p != null)
        {
            fields.Add(p);
            p = p.parent;
        }

        fields.Reverse();

        return fields;
    }

    public void ToggleHasChildren()
    {
        hasChildren = true;
    }

    public override string ToString()
    {
        return string.Join(".", ToList().Select(field => field.field).ToArray());
    }
}
