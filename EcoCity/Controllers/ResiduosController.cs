using EcoCity.Models;
using EcoCity.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcoCity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResiduosController : ControllerBase
{
    private readonly IMongoCollection<ResiduoColeta> _residuosCollection;

    public ResiduosController(IMongoDatabase database)
    {
        _residuosCollection = database.GetCollection<ResiduoColeta>("residuos_coleta");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dados = await _residuosCollection.Find(_ => true).ToListAsync();
        return Ok(dados);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ResiduoColeta novoResiduo)
    {
        novoResiduo.ColetaStatus = EcoCityRegras.CalcularStatusColeta(novoResiduo.VolumeKg, novoResiduo.LimiteKg);

        await _residuosCollection.InsertOneAsync(novoResiduo);
        return CreatedAtAction(nameof(Get), new { id = novoResiduo.Id }, novoResiduo);
    }
}