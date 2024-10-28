using Domain.Services;
using FluentValidation;

namespace Application.UseCases.CreateLimite;

public class CreateLimiteHandler
{
    private readonly ILimiteService _limiteService;
    private readonly IValidator<CreateLimiteCommand> _validator;

    public CreateLimiteHandler(ILimiteService limiteService)
    {
        _limiteService = limiteService;
    }

    public async Task<CreateLimiteResponse> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        // Buscar Se Documento Agencia e Conta existem Se não existirem retorna exception Se valor
        // for menor ou igual a 0 retorna exception
        var validationResult = await _validator.ValidateAsync(command);
        //if (!validationResult.IsValid)
        //{
        //    return Results.BadRequest(validationResult.Errors);
        //}
        var limite = command.ToEntity();
        var entity = await _limiteService.Create(limite, cancellationToken);
        return entity.ToResponse();
    }
}