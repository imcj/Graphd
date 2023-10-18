using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Graph.Proxies;

public class DynamicLinqProxy
{
    public static MethodInfo OrderByMethod = typeof(DynamicQueryableExtensions)
        .GetMethods()
        .Where(m => m.Name == "OrderBy")
        .Where(m => m.ReturnType.IsGenericType)
        .Single(m => m.GetParameters().Length == 3);

    public static MethodInfo[] methodInfos = typeof(DynamicQueryableExtensions)
        .GetMethods()
        .Where(m => m.Name == "OrderBy")
        .Where(m => m.GetParameters().Length == 3)
        .Where(m => m.ReturnType.IsGenericType)
        .ToArray();

    public static Expression OrderBy(Expression expression, Type type, string argument)
    {
        expression = Expression.Convert(expression, typeof(IQueryable));
        var orderBy = Expression.Call(null, OrderByMethod.MakeGenericMethod(type), new Expression[] { expression, Expression.Constant(argument), Expression.NewArrayInit(typeof(object)) });

        return Expression.Convert(
            orderBy,
            typeof(IQueryable<>).MakeGenericType(type)
        );
    }
}
