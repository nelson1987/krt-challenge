namespace Application.UseCases.ChangeLimite;

public record ChangeLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);