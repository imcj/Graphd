using GraphQL.Types;
using System.Linq.Expressions;
using System.Reflection;

namespace Graphd.Graph.Types;

public class ObjectGraphTypeProxy
{

    protected object instance;

    protected Type entityType;

    public ObjectGraphTypeProxy(object instance, Type entityType)
    {

        this.instance = instance;
        this.entityType = entityType;
    }

    public void FieldByLambda(string name, Type fieldType)
    {
        var parameter = Expression.Parameter(entityType, "x");
        var body = Expression.Property(parameter, name);

        var funcType = typeof(Func<,>).MakeGenericType(new Type[] { entityType, fieldType });

        var method = GetLambdaMethod().MakeGenericMethod(new Type[] { funcType });
        var expression = method.Invoke(null, new object[] { body, new ParameterExpression[] { parameter } });

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _ = GetFieldMethod()
            .MakeGenericMethod(new Type[] { fieldType })
            .Invoke(instance, new object[] { expression!, null, null })!;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    protected MethodInfo GetLambdaMethod()
    {
        return typeof(Expression)
            .GetMethods()
            .Where(m => m.GetGenericArguments().Length == 1)
            .ToList()
            .First();
    }

    protected MethodInfo GetFieldMethod()
    {
        var method = instance.GetType()
            .GetMethods()
            .Where(m => m.Name == "Field")
            .Where(m => m.GetGenericArguments().Length == 1)
            .Where(m => m.GetParameters().Length == 3)
            .FirstOrDefault();

        return method ?? throw new Exception("Cannot find the Field method");
    }

    public void AddFieldByName(string name, Type type)
    {
        GetFieldByNameMethod()
            .MakeGenericMethod(type)
            .Invoke(instance, new object[] { name });
    }

    protected MethodInfo GetFieldByNameMethod()
    {
        return instance.GetType()
            .GetMethods()
            .Where(m => m.Name == "Field")
            .Where(m => m.GetGenericArguments().Length == 1)
            .Where(m => m.GetParameters().Length == 1)
            .FirstOrDefault() ?? throw new Exception("Cannot find the field by name Method");
    }
}
