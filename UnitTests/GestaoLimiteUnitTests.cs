namespace UnitTests;

public class UnitTestsBase
{
    public Contexto contexto { get; set; }

    public UnitTestsBase()
    {
        contexto = new Contexto();
    }
}

public class GestaoLimiteUnitTests : UnitTestsBase
{
    [Fact]
    public void AdicionarLimite_DadosValidos_RetornaDadosInseridos()
    {
        var limite = new Limite(0, "Documento", "Agencia", "Conta", 0.00M);
        limite = contexto.Incluir(limite);
        Assert.Equal(1, limite.Id);
        Assert.Equal("Documento", limite.Documento);
        Assert.Equal("Agencia", limite.Agencia);
        Assert.Equal("Conta", limite.Conta);
        Assert.Equal(0.00M, limite.Valor);
    }
}

public class Contexto
{
    public Limite Incluir(Limite limite)
    {
        limite = new Limite(1, limite.Documento, limite.Agencia, limite.Conta, limite.Valor);
        return limite;
    }
}

public class Limite
{
    public Limite(int id, string documento, string agencia, string conta, decimal valor)
    {
        Id = id;
        Documento = documento;
        Agencia = agencia;
        Conta = conta;
        Valor = valor;
    }

    public int Id { get; private set; }
    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Conta { get; private set; }
    public decimal Valor { get; private set; }
}