namespace EcoCity.Services;

public static class EcoCityRegras
{
    public static string CalcularStatusEnergia(int consumoKwh, int limiteKwh)
    {
        if (consumoKwh > limiteKwh) return "Acima do limite";
        if (consumoKwh >= limiteKwh * 0.9) return "Alerta";
        return "Normal";
    }
    
    public static string CalcularStatusColeta(int volumeKg, int limiteKg)
    {
        if (volumeKg >= limiteKg) return "Urgente";
        if (volumeKg >= limiteKg * 0.8) return "Agendar coleta";
        return "Normal";
    }
    
    public static string AvaliarStatusSocial(int quantidadeTreinamentos)
    {
        if (quantidadeTreinamentos >= 10) return "Excelente";
        if (quantidadeTreinamentos >= 5) return "Adequado";
        return "Abaixo da meta";
    }
    
    public static string AvaliarRiscoAmbiental(string licenca, int emissaoCarbonoTon)
    {
        if (licenca == "Vencida" || emissaoCarbonoTon > 150) return "Risco Alto";
        if (licenca == "Pendente") return "Atenção";
        return "Adequado";
    }

    public static string AvaliarAlertaSensor(string tipo, Dictionary<string, int> leitura)
    {
        if (tipo == "temperatura" && leitura.ContainsKey("celsius") && leitura["celsius"] >= 34) return "Alerta de Calor";
        if (tipo == "residuo" && leitura.ContainsKey("nivel") && leitura["nivel"] >= 90) return "Esvaziamento Urgente";
        return "Normal";
    }
}