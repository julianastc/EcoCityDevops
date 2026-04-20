using EcoCity.Models;
using EcoCity.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcoCity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndicadoresSociaisController : ControllerBase
{
    private readonly IMongoCollection<IndicadorSocial> _indicadoresCollection;

    public IndicadoresSociaisController(IMongoDatabase database)
    {
        _indicadoresCollection = database.GetCollection<IndicadorSocial>("indicadores_sociais");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dados = await _indicadoresCollection.Find(_ => true).ToListAsync();
        return Ok(dados);
    }

    [HttpPost]
    public async Task<IActionResult> Post(IndicadorSocial novoIndicador)
    {
        novoIndicador.StatusSocial = EcoCityRegras.AvaliarStatusSocial(novoIndicador.Treinamentos);

        await _indicadoresCollection.InsertOneAsync(novoIndicador);
        return CreatedAtAction(nameof(Get), new { id = novoIndicador.Id }, novoIndicador);
    }
}