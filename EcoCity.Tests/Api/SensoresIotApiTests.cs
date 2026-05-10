using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EcoCity.Tests.Infra;

namespace EcoCity.Tests.Api;

[Collection("Api")]
public class SensoresIotApiTests
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public SensoresIotApiTests(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_sensores_retorna_200()
    {
        var resp = await _client.GetAsync("/api/SensoresIot");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task POST_sensor_temperatura_alta_retorna_Alerta_de_Calor()
    {
        var payload = new
        {
            tipo = "temperatura",
            local = "Praca",
            leitura = new Dictionary<string, int> { { "celsius", 36 } }
        };
        var resp = await _client.PostAsJsonAsync("/api/SensoresIot", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("sensor.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Alerta de Calor", doc.RootElement.GetProperty("alerta").GetString());
    }

    [Fact]
    public async Task POST_sensor_residuo_nivel_alto_retorna_Esvaziamento_Urgente()
    {
        var payload = new
        {
            tipo = "residuo",
            local = "Lixeira 12",
            leitura = new Dictionary<string, int> { { "nivel", 95 } }
        };
        var resp = await _client.PostAsJsonAsync("/api/SensoresIot", payload);

        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Esvaziamento Urgente", doc.RootElement.GetProperty("alerta").GetString());
    }

    [Fact]
    public async Task POST_sensor_temperatura_normal_retorna_Normal()
    {
        var payload = new
        {
            tipo = "temperatura",
            local = "Sala",
            leitura = new Dictionary<string, int> { { "celsius", 22 } }
        };
        var resp = await _client.PostAsJsonAsync("/api/SensoresIot", payload);

        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Normal", doc.RootElement.GetProperty("alerta").GetString());
    }
}
