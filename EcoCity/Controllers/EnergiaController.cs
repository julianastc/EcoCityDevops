using EcoCity.Models;
using EcoCity.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcoCity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnergiaController : ControllerBase
{
    private readonly IMongoCollection<EnergiaConsumo> _energiaCollection;

    public EnergiaController(IMongoDatabase database)
    {
        _energiaCollection = database.GetCollection<EnergiaConsumo>("energia_consumo");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dados = await _energiaCollection.Find(_ => true).ToListAsync();
        return Ok(dados);
    }

    [HttpPost]
    public async Task<IActionResult> Post(EnergiaConsumo novaLeitura)
    {
        novaLeitura.Status = EcoCityRegras.CalcularStatusEnergia(novaLeitura.ConsumoKwh, novaLeitura.LimiteKwh);
        
        await _energiaCollection.InsertOneAsync(novaLeitura);
        return CreatedAtAction(nameof(Get), new { id = novaLeitura.Id }, novaLeitura);
    }
}