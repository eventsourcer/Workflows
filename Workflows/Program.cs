using AsyncHandler.EventSourcing;
using AsyncHandler.EventSourcing.Configuration;

// var builder = WebApplication.CreateBuilder(new WebApplicationOptions
// {
//     EnvironmentName = Environments.Production
// });

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables().AddUserSecrets<Program>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var env = "AzureSqlEnv";
var conn = builder.Configuration[env] ?? throw new Exception($"not connection string found {env}");

builder.Services.AddAsyncHandler(asynchandler =>
{
    asynchandler.AddEventSourcing(source =>
    {
        source.SelectEventSource(EventSources.AzureSql, conn);
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("placeorder", async (IEventSource<OrderAggregate> service) =>
{
    var aggregate = await service.CreateOrRestore();
    aggregate.PlaceOrder(new OrderPlaced());
    await service.Commit(aggregate);
});

app.Run();
