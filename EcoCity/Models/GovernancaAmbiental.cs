using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCity.Models;

public class GovernancaAmbiental
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("empresa")]
    public string Empresa { get; set; } = null!;

    [BsonElement("licenca")]
    public string Licenca { get; set; } = null!;

    [BsonElement("emissao_carbono_ton")]
    public int EmissaoCarbonoTon { get; set; }

    [BsonElement("auditoria")]
    public string Auditoria { get; set; } = null!;

    // Campo calculado pela nossa regra
    [BsonElement("risco_ambiental")]
    public string? RiscoAmbiental { get; set; }
}