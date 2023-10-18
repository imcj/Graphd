using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Graph.Builder;

internal interface ISchameQueryGraphTypeBuilder
{
    IObjectGraphType Build();
}
