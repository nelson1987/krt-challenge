namespace Application.UseCases.CreateLimite;

public record CreateLimiteResponse(string Documento, string Agencia, string Conta, decimal Valor);
public record ChangeLimiteResponse(string Documento, string Agencia, string Conta, decimal Valor);