using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do MongoDB (lendo das variáveis de ambiente do Docker)
var mongoDbConnectionString = builder.Configuration["ConnectionStrings:MongoDb"] ?? "mongodb://localhost:27017";
var databaseName = builder.Configuration["DatabaseName"] ?? "ecocity360";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDbConnectionString));
builder.Services.AddScoped(sp => 
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

// 2. Adicionar suporte a Controllers
builder.Services.AddControllers();

var app = builder.Build();

// 3. Mapear as rotas dos Controllers
app.MapControllers();

// Rota de status mantida para garantir que o seu print antigo ainda funciona
app.MapGet("/api/status", () => new { Status = "EcoCity 360 API Rodando com Controllers!", Versao = "2.0" });

app.Run();