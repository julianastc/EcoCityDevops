using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EcoCity.Tests.Infra;

namespace EcoCity.Tests.Api;

[Collection("Api")]
public class GovernancaAmbientalApiTests
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GovernancaAmbientalApiTests(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_governanca_retorna_200()
    {
        var resp = await _client.GetAsync("/api/GovernancaAmbiental");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task POST_licenca_vencida_retorna_Risco_Alto()
    {
        var payload = new { empresa = "Acme", licenca = "Vencida", emissaoCarbonoTon = 50, auditoria = "OK" };
        var resp = await _client.PostAsJsonAsync("/api/GovernancaAmbiental", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("governanca.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Risco Alto", doc.RootElement.GetProperty("riscoAmbiental").GetString());
    }

    [Fact]
    public async Task POST_licenca_ativa_baixa_emissao_retorna_Adequado()
    {
        var payload = new { empresa = "Beta", licenca = "Ativa", emissaoCarbonoTon = 50, auditoria = "OK" };
        var resp = await _client.PostAsJsonAsync("/api/GovernancaAmbiental", payload);

        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Adequado", doc.RootElement.GetProperty("riscoAmbiental").GetString());
    }
}
