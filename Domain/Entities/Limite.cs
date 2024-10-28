namespace Domain.Entities;

public class Limite
{
    public Limite(string documento, string agencia, string conta, decimal valor)
    {
        Id = Guid.NewGuid();
        Documento = documento;
        Agencia = agencia;
        Conta = conta;
        Valor = valor;
    }

    public Guid Id { get; private set; }
    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Conta { get; private set; }
    public decimal Valor { get; private set; }
}