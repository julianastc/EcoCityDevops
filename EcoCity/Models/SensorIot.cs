using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace EcoCity.Models;

public class SensorIot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("tipo")]
    public string Tipo { get; set; } = null!;

    [BsonElement("local")]
    public string Local { get; set; } = null!;

    // Usamos um Dicionário porque a leitura pode ser {celsius: 35} ou {tensao: 220, corrente: 30}
    [BsonElement("leitura")]
    public Dictionary<string, int> Leitura { get; set; } = new();

    // Campo calculado pela nossa regra
    [BsonElement("alerta")]
    public string? Alerta { get; set; }
}