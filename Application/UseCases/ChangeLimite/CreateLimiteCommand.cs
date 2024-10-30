namespace Application.UseCases.ChangeLimite;

public record CreateLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);