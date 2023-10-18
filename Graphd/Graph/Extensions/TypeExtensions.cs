namespace Graphd.Graph.Extensions;

public static class TypeExtensions
{
    static readonly string[] Types = new string[] { "List`", "HashSet`", "DbSet`" };

    public static bool IsRelationship(this Type type)
    {
        if (type.Namespace != "System.Collections.Generic")
        {
            return false;
        }

        return Types.Any(name => type.Name.StartsWith(name));
    }
}
