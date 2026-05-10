using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EcoCity.Tests.Infra;

namespace EcoCity.Tests.Api;

[Collection("Api")]
public class ResiduosApiTests
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ResiduosApiTests(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_residuos_retorna_200()
    {
        var resp = await _client.GetAsync("/api/Residuos");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task POST_residuo_volume_baixo_retorna_status_Normal()
    {
        var payload = new { bairro = "Centro", tipo = "organico", volumeKg = 100, limiteKg = 500 };

        var resp = await _client.PostAsJsonAsync("/api/Residuos", payload);
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("residuo.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Normal", doc.RootElement.GetProperty("coletaStatus").GetString());
    }

    [Fact]
    public async Task POST_residuo_volume_acima_retorna_status_Urgente()
    {
        var payload = new { bairro = "Vila", tipo = "reciclavel", volumeKg = 600, limiteKg = 500 };

        var resp = await _client.PostAsJsonAsync("/api/Residuos", payload);

        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("residuo.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Urgente", doc.RootElement.GetProperty("coletaStatus").GetString());
    }

    [Fact]
    public async Task POST_residuo_payload_invalido_retorna_400()
    {
        var content = new StringContent("nao-eh-json", System.Text.Encoding.UTF8, "application/json");
        var resp = await _client.PostAsync("/api/Residuos", content);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }
}
