using EcoCity.Services;
using Xunit;

namespace EcoCity.Tests;

public class RegrasTests
{
    [Fact]
    public void CalcularStatusEnergia_DeveRetornarAcimaDoLimite_QuandoConsumoExcede()
    {
        var resultado = EcoCityRegras.CalcularStatusEnergia(1200, 1000);
        Assert.Equal("Acima do limite", resultado);
    }
    
    [Fact]
    public void CalcularStatusEnergia_DeveRetornarAlerta_QuandoConsumoProximoDoLimite()
    {
        var resultado = EcoCityRegras.CalcularStatusEnergia(950, 1000);
        Assert.Equal("Alerta", resultado);
    }

    [Fact]
    public void CalcularStatusEnergia_DeveRetornarNormal_QuandoConsumoBaixo()
    {
        var resultado = EcoCityRegras.CalcularStatusEnergia(800, 1000);
        Assert.Equal("Normal", resultado);
    }
    
    [Fact]
    public void CalcularStatusColeta_DeveRetornarUrgente_QuandoVolumeMaiorOuIgualAoLimite()
    {
        // Arrange & Act
        var resultado = EcoCityRegras.CalcularStatusColeta(600, 500);
        
        // Assert
        Assert.Equal("Urgente", resultado);
    }

    [Fact]
    public void CalcularStatusColeta_DeveRetornarAgendarColeta_QuandoVolumeProximoAoLimite()
    {
        var resultado = EcoCityRegras.CalcularStatusColeta(450, 500);
        Assert.Equal("Agendar coleta", resultado);
    }
    
    [Fact]
    public void AvaliarStatusSocial_DeveRetornarExcelente_QuandoTreinamentosMaiorOuIgualA10()
    {
        var resultado = EcoCityRegras.AvaliarStatusSocial(12);
        Assert.Equal("Excelente", resultado);
    }

    [Fact]
    public void AvaliarStatusSocial_DeveRetornarAbaixoDaMeta_QuandoTreinamentosMenorQue5()
    {
        var resultado = EcoCityRegras.AvaliarStatusSocial(3);
        Assert.Equal("Abaixo da meta", resultado);
    }
    
    [Fact]
    public void AvaliarRiscoAmbiental_DeveRetornarRiscoAlto_QuandoLicencaVencida()
    {
        var resultado = EcoCityRegras.AvaliarRiscoAmbiental("Vencida", 100);
        Assert.Equal("Risco Alto", resultado);
    }

    [Fact]
    public void AvaliarRiscoAmbiental_DeveRetornarRiscoAlto_QuandoEmissaoMaiorQue150()
    {
        var resultado = EcoCityRegras.AvaliarRiscoAmbiental("Ativa", 200);
        Assert.Equal("Risco Alto", resultado);
    }

    [Fact]
    public void AvaliarAlertaSensor_DeveRetornarAlertaCalor_QuandoTemperaturaAlta()
    {
        var leitura = new System.Collections.Generic.Dictionary<string, int> { { "celsius", 35 } };
        var resultado = EcoCityRegras.AvaliarAlertaSensor("temperatura", leitura);
        Assert.Equal("Alerta de Calor", resultado);
    }

    [Fact]
    public void AvaliarAlertaSensor_DeveRetornarEsvaziamentoUrgente_QuandoNivelLixoAlto()
    {
        var leitura = new System.Collections.Generic.Dictionary<string, int> { { "nivel", 95 } };
        var resultado = EcoCityRegras.AvaliarAlertaSensor("residuo", leitura);
        Assert.Equal("Esvaziamento Urgente", resultado);
    }
}