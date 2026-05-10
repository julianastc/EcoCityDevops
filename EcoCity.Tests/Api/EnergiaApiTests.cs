using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EcoCity.Tests.Infra;

namespace EcoCity.Tests.Api;

[Collection("Api")]
public class EnergiaApiTests
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EnergiaApiTests(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_energia_retorna_200_e_array_valido()
    {
        var resp = await _client.GetAsync("/api/Energia");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }

    [Fact]
    public async Task POST_energia_consumo_normal_retorna_201_e_status_Normal()
    {
        var payload = new { local = "Predio A", consumoKwh = 500, limiteKwh = 1000, data = "2026-05-10" };

        var resp = await _client.PostAsJsonAsync("/api/Energia", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("energia.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Normal", doc.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task POST_energia_consumo_acima_retorna_status_Acima_do_limite()
    {
        var payload = new { local = "Predio B", consumoKwh = 1500, limiteKwh = 1000, data = "2026-05-10" };

        var resp = await _client.PostAsJsonAsync("/api/Energia", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Acima do limite", doc.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task POST_energia_payload_invalido_retorna_400()
    {
        var content = new StringContent("{ invalido", System.Text.Encoding.UTF8, "application/json");
        var resp = await _client.PostAsync("/api/Energia", content);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact]
    public async Task GET_apos_POST_retorna_array_que_atende_ao_schema()
    {
        var payload = new { local = "Predio C", consumoKwh = 300, limiteKwh = 1000, data = "2026-05-10" };
        await _client.PostAsJsonAsync("/api/Energia", payload);

        var resp = await _client.GetAsync("/api/Energia");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarArrayAsync("energia.schema.json", body);
    }
}
