namespace Application.UseCases.CreateLimite;

public record CreateLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);
public record GetLimiteQuery(string Documento, string Agencia, string Conta, decimal Valor);
public record ChangeLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);
public record DeleteLimiteCommand(string Documento, string Conta);