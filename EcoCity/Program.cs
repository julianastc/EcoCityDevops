var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/status", () => new { Status = "EcoCity 360 API Rodando!", Versao = "1.0" });

app.MapGet("/api/energia/analise", (int consumo, int limite) => 
{
    var status = EcoCityRegras.AnalisarConsumo(consumo, limite);
    return new { Consumo = consumo, Limite = limite, Status = status };
});

app.Run();

public static class EcoCityRegras
{
    public static string AnalisarConsumo(int consumoKwh, int limiteKwh)
    {
        if (consumoKwh <= limiteKwh * 0.8) return "Excelente";
        if (consumoKwh <= limiteKwh) return "Normal";
        return "Acima do limite";
    }
}