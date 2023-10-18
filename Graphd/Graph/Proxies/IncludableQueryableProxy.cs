using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Graph.Proxies;

public class IncludableQueryableProxy
{
    public static MethodInfo includeMethod = typeof(EntityFrameworkQueryableExtensions)

        .GetMethods()
        .Where(m => m.Name == "Include")
        .Single(m => m.GetParameters().Any(p => p.Name == "navigationPropertyPath" && p.ParameterType.Name.StartsWith("Expression`")));

    public static MethodInfo[] methods = typeof(EntityFrameworkQueryableExtensions).GetMethods();
}
