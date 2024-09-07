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
    public async Task TestMsSqlClient()
    {
        // giveb
        var sp = BuildCOntainer();
        var source = EventSources.SqlServer;
        var conn = BuildConfiguration(source);
        var client = new AzureSqlClient<OrderAggregate>(conn, sp, source);
        await client.Init();

        //when
        var aggregate = await client.CreateOrRestore();

        // then
        Assert.NotNull(aggregate);
    }
    [Fact]
    public async Task TestAzureSqlClient()
    {
        // giveb
        var source = EventSources.SqlServer;
        var sp = BuildCOntainer();
        var conn = BuildConfiguration(source);
        var client = new AzureSqlClient<OrderAggregate>(conn, sp, source);
        await client.Init();

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
        configs.Add(EventSources.SqlServer, new SqlServerConfig());
        services.AddKeyedSingleton("SourceConfig", configs);
        return services.BuildServiceProvider();
    }
    private static string BuildConfiguration(EventSources source)
    {
        var builder = new ConfigurationBuilder();
        builder.AddUserSecrets<UnitTest1>()
        .AddEnvironmentVariables();
        return source switch
        {
            EventSources.SqlServer => builder.Build().GetValue<string>("mssqlenv") ??
                throw new Exception("no connection string found"),
            EventSources.AzureSql => builder.Build().GetValue<string>("azuresqlenv") ??
                throw new Exception("no connection string found"),
            _ => string.Empty
        };
    }
}
