using GraphQL.Types;
using Graphd.Graph.Types;
using Graphd.Tests.Models;


namespace Graphd.Tests;

public class ObjectGraphTypeProxyTest
{
    [Fact]
    public void TestFieldByName()
    {
        var droid = new ObjectGraphType<Droid>();
        var proxy = new ObjectGraphTypeProxy(droid, droid.GetType());

        var fieldType = typeof(ObjectGraphType<Factory>);

        proxy.AddFieldByName("factory", fieldType);
    }

    [Fact]
    public void TestFieldByLambda()
    {
        var droid = new ObjectGraphType<Droid>();
        var proxy = new ObjectGraphTypeProxy(droid, typeof(Droid));

        proxy.FieldByLambda("Id", typeof(string));
    }
}
