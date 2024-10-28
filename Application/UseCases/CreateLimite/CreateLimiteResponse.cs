namespace Application.UseCases.CreateLimite;

public record CreateLimiteResponse(Guid Id, string Documento, string Agencia, string Conta, decimal Valor);