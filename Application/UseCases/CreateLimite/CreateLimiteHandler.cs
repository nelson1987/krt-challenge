using Domain.Services;
using FluentResults;
using FluentValidation;

namespace Application.UseCases.CreateLimite;

public interface ICreateLimiteHandler
{
    Task<Result<CreateLimiteResponse>> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default);
}

public class CreateLimiteHandler : ICreateLimiteHandler
{
    private readonly ILimiteService _limiteService;
    private readonly IValidator<CreateLimiteCommand> _validator;

    public CreateLimiteHandler(ILimiteService limiteService, IValidator<CreateLimiteCommand> validator)
    {
        _limiteService = limiteService;
        _validator = validator;
    }

    public async Task<Result<CreateLimiteResponse>> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        // Buscar Se Documento Agencia e Conta existem Se não existirem retorna exception Se valor
        // for menor ou igual a 0 retorna exception

        var limite = command.ToEntity();
        var entity = await _limiteService.Create(limite, cancellationToken);
        return entity.ToResponse();
    }
}