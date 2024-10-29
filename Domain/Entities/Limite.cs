namespace Domain.Entities;

public class Limite
{
    public Limite(string documento, string agencia, string conta, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(documento)) throw new ArgumentException("documento");
        Documento = documento;
        if (string.IsNullOrWhiteSpace(agencia)) throw new ArgumentException("agencia");
        Agencia = agencia;
        if (string.IsNullOrWhiteSpace(conta)) throw new ArgumentException("conta");
        Conta = conta;
        if (valor <= 0) throw new ArgumentException("valor");
        Valor = valor;
    }
    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Conta { get; private set; }
    public decimal Valor { get; private set; }
}