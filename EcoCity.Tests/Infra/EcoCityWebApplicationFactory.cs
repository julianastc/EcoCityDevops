using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace EcoCity.Tests.Infra;

public class EcoCityWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly Lazy<MongoDbContainer> SharedContainer = new(() =>
    {
        var container = new MongoDbBuilder().WithImage("mongo:7.0").Build();
        container.StartAsync().GetAwaiter().GetResult();
        return container;
    });

    public string DatabaseName { get; } = "ecocity_tests";
    private string ConnectionString => SharedContainer.Value.GetConnectionString();

    public Task InitializeAsync()
    {
        _ = SharedContainer.Value;
        return Task.CompletedTask;
    }

    public new Task DisposeAsync() => Task.CompletedTask;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var clientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));
            if (clientDescriptor != null) services.Remove(clientDescriptor);

            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDatabase));
            if (dbDescriptor != null) services.Remove(dbDescriptor);

            services.AddSingleton<IMongoClient>(new MongoClient(ConnectionString));
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(DatabaseName);
            });
        });
    }

    public void ResetDatabase()
    {
        var client = new MongoClient(ConnectionString);
        client.DropDatabase(DatabaseName);
    }
}
