using Graphd.Graph.Types;
using Graphd.Tests.Models;

namespace Graphd.Tests;

public class GraphTypeFactoryTest
{
    [Fact]
    public void TestMakeEntityGraphType()
    {
        var factory = new GraphTypeFactory();
        var droidGraphType = factory.MakeGraphType(typeof(Droid));

        var a1 = typeof(EntityObjectGraphType<Droid>);

        Assert.True(a1 == droidGraphType);
        Assert.False(typeof(string) == droidGraphType);
    }

    [Fact]
    public void TestMakeListGraphType()
    {
        var factory = new GraphTypeFactory();
        var droidGraphType = factory.MakeGraphType(typeof(Droid));
        var droidListGraphType = factory.MakeListGraphType(droidGraphType);
    }
}
