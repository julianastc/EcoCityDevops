using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EcoCity.Tests.Infra;

namespace EcoCity.Tests.Api;

[Collection("Api")]
public class IndicadoresSociaisApiTests
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public IndicadoresSociaisApiTests(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GET_indicadores_retorna_200()
    {
        var resp = await _client.GetAsync("/api/IndicadoresSociais");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task POST_indicador_treinamentos_altos_retorna_Excelente()
    {
        var payload = new { empresa = "Acme", diversidadeGenero = "Alta", inclusaoPcd = "Sim", treinamentos = 12 };
        var resp = await _client.PostAsJsonAsync("/api/IndicadoresSociais", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var body = await resp.Content.ReadAsStringAsync();
        await SchemaValidator.ValidarObjetoAsync("indicadorSocial.schema.json", body);

        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Excelente", doc.RootElement.GetProperty("statusSocial").GetString());
    }

    [Fact]
    public async Task POST_indicador_poucos_treinamentos_retorna_AbaixoDaMeta()
    {
        var payload = new { empresa = "Beta", diversidadeGenero = "Media", inclusaoPcd = "Nao", treinamentos = 2 };
        var resp = await _client.PostAsJsonAsync("/api/IndicadoresSociais", payload);

        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        Assert.Equal("Abaixo da meta", doc.RootElement.GetProperty("statusSocial").GetString());
    }
}
