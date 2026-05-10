using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EcoCity.Tests.Infra;
using Reqnroll;

namespace EcoCity.Tests.Steps;

[Binding]
public class ApiSteps : IClassFixture<EcoCityWebApplicationFactory>
{
    private readonly EcoCityWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private HttpResponseMessage? _response;
    private string _body = string.Empty;
    private object? _payload;

    public ApiSteps(EcoCityWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.ResetDatabase();
        _client = _factory.CreateClient();
    }

    // ---------- Energia ----------
    [Given(@"que envio uma leitura de energia com consumo (\d+) kWh e limite (\d+) kWh")]
    public void DadoLeituraEnergia(int consumo, int limite)
    {
        _payload = new { local = "Predio Teste", consumoKwh = consumo, limiteKwh = limite, data = "2026-05-10" };
    }

    [Given(@"que envio um payload de energia inválido")]
    public void DadoPayloadEnergiaInvalido()
    {
        _payload = "{ payload-invalido";
    }

    // ---------- Resíduos ----------
    [Given(@"que envio uma coleta no bairro ""(.*)"" com volume (\d+) kg e limite (\d+) kg")]
    public void DadoColetaResiduo(string bairro, int volume, int limite)
    {
        _payload = new { bairro, tipo = "organico", volumeKg = volume, limiteKg = limite };
    }

    // ---------- Sensores ----------
    [Given(@"que envio uma leitura do sensor ""(.*)"" no local ""(.*)"" com chave ""(.*)"" valor (\d+)")]
    public void DadoLeituraSensor(string tipo, string local, string chave, int valor)
    {
        _payload = new
        {
            tipo,
            local,
            leitura = new Dictionary<string, int> { { chave, valor } }
        };
    }

    // ---------- Ações ----------
    [When(@"a requisição POST é enviada para ""(.*)""")]
    public async Task QuandoPost(string url)
    {
        if (_payload is string raw)
        {
            var content = new StringContent(raw, Encoding.UTF8, "application/json");
            _response = await _client.PostAsync(url, content);
        }
        else
        {
            _response = await _client.PostAsJsonAsync(url, _payload);
        }
        _body = await _response.Content.ReadAsStringAsync();
    }

    [When(@"a requisição GET é enviada para ""(.*)""")]
    public async Task QuandoGet(string url)
    {
        _response = await _client.GetAsync(url);
        _body = await _response.Content.ReadAsStringAsync();
    }

    // ---------- Asserts ----------
    [Then(@"o código de resposta deve ser (\d+)")]
    public void EntaoStatusCode(int codigo)
    {
        Assert.NotNull(_response);
        Assert.Equal(codigo, (int)_response!.StatusCode);
    }

    [Then(@"o campo ""(.*)"" da resposta deve ser ""(.*)""")]
    public void EntaoCampoResposta(string campo, string valorEsperado)
    {
        using var doc = JsonDocument.Parse(_body);
        Assert.Equal(valorEsperado, doc.RootElement.GetProperty(campo).GetString());
    }

    [Then(@"a resposta deve respeitar o schema ""(.*)""")]
    public async Task EntaoSchema(string nomeSchema)
    {
        await SchemaValidator.ValidarObjetoAsync(nomeSchema, _body);
    }

    [Then(@"o corpo da resposta deve ser uma lista JSON")]
    public void EntaoListaJson()
    {
        using var doc = JsonDocument.Parse(_body);
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }
}
