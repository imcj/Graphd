using Graphd.HTTPServer.Extensions;
using Graphd.Tests.Models;

namespace Graphd.HTTPServer;

public class GraphdHostedService : IHostedService
{
    protected IServiceScopeFactory serviceScopeFactory;

    public GraphdHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<GraphDbContext>();
        dbContext?.Initialize();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
