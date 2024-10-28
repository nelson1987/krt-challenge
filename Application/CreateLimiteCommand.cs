namespace Application;

public record CreateLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);
public record CreateLimiteResponse(Guid Id, string Documento, string Agencia, string Conta, decimal Valor);

public class CreateLimiteHandler
{
    public async Task<CreateLimiteResponse> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        // Buscar Se Documento Agencia e Conta existem Se não existirem retorna exception Se valor
        // for menor ou igual a 0
        throw new NotImplementedException();
    }
}