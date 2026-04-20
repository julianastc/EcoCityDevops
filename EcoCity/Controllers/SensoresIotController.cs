using EcoCity.Models;
using EcoCity.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EcoCity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensoresIotController : ControllerBase
{
    private readonly IMongoCollection<SensorIot> _sensoresCollection;

    public SensoresIotController(IMongoDatabase database)
    {
        _sensoresCollection = database.GetCollection<SensorIot>("sensores_iot");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dados = await _sensoresCollection.Find(_ => true).ToListAsync();
        return Ok(dados);
    }

    [HttpPost]
    public async Task<IActionResult> Post(SensorIot novoSensor)
    {
        novoSensor.Alerta = EcoCityRegras.AvaliarAlertaSensor(novoSensor.Tipo, novoSensor.Leitura);
        await _sensoresCollection.InsertOneAsync(novoSensor);
        return CreatedAtAction(nameof(Get), new { id = novoSensor.Id }, novoSensor);
    }
}