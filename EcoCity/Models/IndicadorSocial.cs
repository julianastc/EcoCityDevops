using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCity.Models;

public class IndicadorSocial
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("empresa")]
    public string Empresa { get; set; } = null!;

    [BsonElement("diversidade_genero")]
    public string DiversidadeGenero { get; set; } = null!;

    [BsonElement("inclusao_pcd")]
    public string InclusaoPcd { get; set; } = null!;

    [BsonElement("treinamentos")]
    public int Treinamentos { get; set; }

    // Campo extra para guardarmos o resultado da nossa regra de negócio
    [BsonElement("status_social")]
    public string? StatusSocial { get; set; }
}