using Domain.Services;

namespace Application;

public record CreateLimiteCommand(string Documento, string Agencia, string Conta, decimal Valor);
public record CreateLimiteResponse(Guid Id, string Documento, string Agencia, string Conta, decimal Valor);

public class CreateLimiteHandler
{
    private readonly ILimiteService _limiteService;

    public CreateLimiteHandler(ILimiteService limiteService)
    {
        _limiteService = limiteService;
    }

    public async Task<CreateLimiteResponse> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        // Buscar Se Documento Agencia e Conta existem Se não existirem retorna exception Se valor
        // for menor ou igual a 0 retorna exception
        var limite = command.ToEntity();
        var entity = await _limiteService.Create(limite, cancellationToken);
        return entity.ToResponse();
    }
}