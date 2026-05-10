using Mongo2Go;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EcoCity.Tests.Infra;

public class EcoCityWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly MongoDbRunner _runner;
    public string DatabaseName { get; } = "ecocity_tests";

    public EcoCityWebApplicationFactory()
    {
        _runner = MongoDbRunner.Start(singleNodeReplSet: false);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var clientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));
            if (clientDescriptor != null) services.Remove(clientDescriptor);

            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDatabase));
            if (dbDescriptor != null) services.Remove(dbDescriptor);

            services.AddSingleton<IMongoClient>(new MongoClient(_runner.ConnectionString));
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(DatabaseName);
            });
        });
    }

    public void ResetDatabase()
    {
        var client = new MongoClient(_runner.ConnectionString);
        client.DropDatabase(DatabaseName);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _runner.Dispose();
        }
        base.Dispose(disposing);
    }
}
