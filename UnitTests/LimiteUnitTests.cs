using Domain.Entities;

namespace UnitTests;

public class LimiteUnitTests
{
    [Fact]
    public void InstanciarLimite_DadosValidos_RetornaDados()
    {
        var limite = new Limite("Documento", "Agencia", "Conta", 0.01M);
        Assert.NotNull(limite);
        Assert.Equal("Documento", limite.Documento);
        Assert.Equal("Agencia", limite.Agencia);
        Assert.Equal("Conta", limite.Conta);
        Assert.Equal(0.01M, limite.Valor);
    }

    [Theory]
    [InlineData("", "Agencia", "Conta", "0.01")]
    [InlineData(" ", "Agencia", "Conta", "0.01")]
    [InlineData(null, "Agencia", "Conta", "0.01")]
    [InlineData("Documento", "", "Conta", "0.01")]
    [InlineData("Documento", " ", "Conta", "0.01")]
    [InlineData("Documento", null, "Conta", "0.01")]
    [InlineData("Documento", "Agencia", "", "0.01")]
    [InlineData("Documento", "Agencia", " ", "0.01")]
    [InlineData("Documento", "Agencia", null, "0.01")]
    [InlineData("Documento", "Agencia", "Conta", "0.00")]
    [InlineData("Documento", "Agencia", "Conta", "-0.01")]
    public void InstanciarLimite_DadosInvalidos_DisparaExcecao(string documento, string agencia, string conta, string valor)
    {
        Assert.Throws<ArgumentException>(() => new Limite(documento, agencia, conta, Convert.ToDecimal(valor)));
    }
}