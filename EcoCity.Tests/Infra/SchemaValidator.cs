using NJsonSchema;

namespace EcoCity.Tests.Infra;

public static class SchemaValidator
{
    private static readonly string SchemasDir =
        Path.Combine(AppContext.BaseDirectory, "Schemas");

    public static async Task ValidarObjetoAsync(string nomeArquivo, string json)
    {
        var schemaPath = Path.Combine(SchemasDir, nomeArquivo);
        var schema = await JsonSchema.FromFileAsync(schemaPath);
        var erros = schema.Validate(json);
        Assert.True(erros.Count == 0,
            $"JSON nao atende ao schema {nomeArquivo}: {string.Join("; ", erros)}");
    }

    public static async Task ValidarArrayAsync(string nomeArquivo, string jsonArray)
    {
        var schemaPath = Path.Combine(SchemasDir, nomeArquivo);
        var schema = await JsonSchema.FromFileAsync(schemaPath);

        using var doc = System.Text.Json.JsonDocument.Parse(jsonArray);
        Assert.Equal(System.Text.Json.JsonValueKind.Array, doc.RootElement.ValueKind);

        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var erros = schema.Validate(item.GetRawText());
            Assert.True(erros.Count == 0,
                $"Item do array nao atende ao schema {nomeArquivo}: {string.Join("; ", erros)}");
        }
    }
}
