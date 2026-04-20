using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCity.Models;

public class EnergiaConsumo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("local")]
    public string Local { get; set; } = null!;

    [BsonElement("consumo_kwh")]
    public int ConsumoKwh { get; set; }

    [BsonElement("limite_kwh")]
    public int LimiteKwh { get; set; }

    [BsonElement("data")]
    public string Data { get; set; } = null!;

    [BsonElement("status")]
    public string? Status { get; set; }
}