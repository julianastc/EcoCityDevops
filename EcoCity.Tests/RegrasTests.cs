using Xunit;

namespace EcoCity.Tests;

public class RegrasTests
{
    [Fact]
    public void AnalisarConsumo_DeveRetornarAcimaDoLimite_QuandoConsumoMaiorQueLimite()
    {
        // Arrange
        int consumoHospital = 1200;
        int limiteHospital = 1000;

        // Act
        var resultado = EcoCityRegras.AnalisarConsumo(consumoHospital, limiteHospital);

        // Assert
        Assert.Equal("Acima do limite", resultado);
    }
    
    [Fact]
    public void AnalisarConsumo_DeveRetornarNormal_QuandoConsumoIgualLimite()
    {
        var resultado = EcoCityRegras.AnalisarConsumo(1000, 1000);
        Assert.Equal("Normal", resultado);
    }
}