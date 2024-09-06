using System.Diagnostics.Tracing;
using AsyncHandler.EventSourcing.Configuration;
using AsyncHandler.EventSourcing.Repositories.AzureSql;
using AsyncHandler.EventSourcing.SourceConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Workflows.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.Equal(1,1);
    }
    [Fact]
    public void Test2()
    {
        List<int> folks = [1, 2, 3];
        Assert.Equal(3, folks.Count);
    }
    [Fact]
    public async Task TestAzureSqlClient()
    {
        // giveb
        var sp = BuildCOntainer();
        var conn = BuildConfiguration();
        var client = new AzureSqlClient<OrderAggregate>(conn, sp, EventSources.AzureSql);

        //when
        var aggregate = await client.CreateOrRestore();

        // then
        Assert.NotNull(aggregate);
    }
    private IServiceProvider BuildCOntainer()
    {
        var services = new ServiceCollection();
        var factory = new LoggerFactory();
        services.AddSingleton<ILogger<AzureSqlClient<OrderAggregate>>>(new Logger<AzureSqlClient<OrderAggregate>>(factory));
        Dictionary<EventSources,IClientConfig> configs = new();
        configs.Add(EventSources.AzureSql, new AzureSqlConfig());
        services.AddKeyedSingleton("SourceConfig", configs);
        return services.BuildServiceProvider();
    }
    private static string BuildConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.AddUserSecrets<UnitTest1>();
        return builder.Build().GetValue<string>("AzureSqlDatabase") ??
        throw new Exception("no connection string found");
    }
}
