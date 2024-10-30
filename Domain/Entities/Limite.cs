using Domain.Helpers;

namespace Domain.Entities;

public class Limite
{
    public Limite(string documento, string agencia, string conta, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(documento)) throw new ArgumentException(Mensagem.LIMITE_INSTANCIADO_SEM_DOCUMENTO);
        Documento = documento;
        if (string.IsNullOrWhiteSpace(agencia)) throw new ArgumentException(Mensagem.LIMITE_INSTANCIADO_SEM_AGENCIA);
        Agencia = agencia;
        if (string.IsNullOrWhiteSpace(conta)) throw new ArgumentException(Mensagem.LIMITE_INSTANCIADO_SEM_CONTA);
        Conta = conta;
        if (valor <= 0) throw new ArgumentException(Mensagem.LIMITE_INSTANCIADO_SEM_VALOR);
        Valor = valor;
    }

    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Conta { get; private set; }
    public decimal Valor { get; private set; }
}