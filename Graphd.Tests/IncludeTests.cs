using Graphd.Graph.Proxies;
using Graphd.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Parser;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graphd.Tests;

public class IncludeTests
{
    [Fact]
    public async Task TestInclude()
    {
        var dbContext = new GraphDbContext(new DbContextOptionsBuilder().UseInMemoryDatabase("test").Options);
        dbContext.Database.EnsureCreated();

        dbContext.AddRange(
            new Droid()
            {
                Id = "1",
                Name = "R2-D2",
                Factory = new()
                {
                    Id = "1",
                    Name = "R2",
                    Droids = new List<Droid>() { new Droid() { Id = "4", Name = "R2-D4" } }
                },
                Tags = new()
                {
                    new Tag() { Id = 1, Name = "R2" },
                    new Tag() { Id = 2, Name = "D2" },
                }
            },
            new Droid()
            {
                Id = "2",
                Name = "R2-D3",
            }
            );

        await dbContext.SaveChangesAsync();
        dbContext.Heros.OrderBy("id desc").Where("Id = 1");
        var q2 = dbContext.Heros.Include(x => x.Tags.OrderBy(y => y.Id));

        var q3 = dbContext.Heros.Include(x => (x.Tags as IQueryable<Tag>).OrderBy("Id desc"));

        //var e1 = DynamicExpressionParser.ParseLambda(new object[] {}, );

        var p = new ExpressionParser(null, "Id desc", null, null);

        var q4 = q3.ToList();
        var f = (dbContext.Heros as IQueryable).OrderBy("Id desc").ToDynamicList();

        //new ExpressionParser(null, typeof(Tag), )
        //dbContext.Heros.Include(d => d.Tags.Take(5));

        var m = DynamicLinqProxy.methodInfos;

        var parameter = Expression.Parameter(typeof(Droid), "x");
        var property = Expression.Property(parameter, "Tags");
        var body = DynamicLinqProxy.OrderBy(property, typeof(Droid), "Id desc");
        var lambda = Expression.Lambda<Func<Droid, IEnumerable<Tag>>>(body, new ParameterExpression[] { parameter });

        var d = dbContext.Heros.Include(lambda);
    }
}
