using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCity.Models;

public class ResiduoColeta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("bairro")]
    public string Bairro { get; set; } = null!;

    [BsonElement("tipo")]
    public string Tipo { get; set; } = null!;

    [BsonElement("volume_kg")]
    public int VolumeKg { get; set; }

    [BsonElement("limite_kg")]
    public int LimiteKg { get; set; }

    [BsonElement("coleta_status")]
    public string? ColetaStatus { get; set; }
}