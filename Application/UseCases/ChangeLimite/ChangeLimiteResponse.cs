namespace Application.UseCases.ChangeLimite;

public record ChangeLimiteResponse(string Documento, string Agencia, string Conta, decimal Valor);