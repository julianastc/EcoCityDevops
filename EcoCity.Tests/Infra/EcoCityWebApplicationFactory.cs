using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EcoCity.Tests.Infra;

public class EcoCityWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("TEST_MONGODB_CONNECTION")
        ?? "mongodb://localhost:27017";

    public string DatabaseName { get; } = $"ecocity_tests_{Guid.NewGuid():N}";

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
