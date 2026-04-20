using EcoCity.Models;
using EcoCity.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcoCity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GovernancaAmbientalController : ControllerBase
{
    private readonly IMongoCollection<GovernancaAmbiental> _governancaCollection;

    public GovernancaAmbientalController(IMongoDatabase database)
    {
        _governancaCollection = database.GetCollection<GovernancaAmbiental>("governanca_ambiental");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dados = await _governancaCollection.Find(_ => true).ToListAsync();
        return Ok(dados);
    }

    [HttpPost]
    public async Task<IActionResult> Post(GovernancaAmbiental novaGovernanca)
    {
        novaGovernanca.RiscoAmbiental = EcoCityRegras.AvaliarRiscoAmbiental(novaGovernanca.Licenca, novaGovernanca.EmissaoCarbonoTon);
        await _governancaCollection.InsertOneAsync(novaGovernanca);
        return CreatedAtAction(nameof(Get), new { id = novaGovernanca.Id }, novaGovernanca);
    }
}