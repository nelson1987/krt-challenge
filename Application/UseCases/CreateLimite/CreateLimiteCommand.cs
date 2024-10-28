namespace Application.UseCases.CreateLimite;

public record CreateLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);