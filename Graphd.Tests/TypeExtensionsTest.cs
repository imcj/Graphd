using Graphd.Graph.Extensions;

namespace Graphd.Tests;

public class TypeExtensionsTest
{
    [Fact]
    public void TestIsRelationship()
    {
        var type1 = typeof(List<int>);
        var type2 = typeof(List<>);
        var type3 = typeof(HashSet<int>);
        var type4 = typeof(HashSet<>);

        Assert.True(type1.IsRelationship());
        Assert.True(type2.IsRelationship());
        Assert.True(type3.IsRelationship());
        Assert.True(type4.IsRelationship());
        Assert.False(typeof(string).IsRelationship());
    }
}
