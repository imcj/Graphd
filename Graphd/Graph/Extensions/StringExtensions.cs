namespace Graphd.Graph.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string self)
    {
        return $"{self[..1].ToUpper()}{self[1..]}";
    }
}
