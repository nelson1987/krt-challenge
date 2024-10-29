using Domain.Services;
using FluentResults;

namespace Application.UseCases.CreateLimite;

public class CreateLimiteHandler : ICreateLimiteHandler
{
    private readonly ILimiteService _limiteService;

    public CreateLimiteHandler(ILimiteService limiteService)
    {
        _limiteService = limiteService;
    }

    public async Task<Result<CreateLimiteResponse>> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        var limite = command.ToEntity();
        var entity = await _limiteService.Create(limite, cancellationToken);
        return Result.Ok(entity.ToCreateResponse());
    }
}

public class ChangeLimiteHandler : IChangeLimiteHandler
{
    private readonly ILimiteService _limiteService;

    public ChangeLimiteHandler(ILimiteService limiteService)
    {
        _limiteService = limiteService;
    }

    public async Task<Result<ChangeLimiteResponse>> Handle(ChangeLimiteCommand command, CancellationToken cancellationToken = default)
    {
        var limite = command.ToEntity();
        var entity = await _limiteService.Update(limite, cancellationToken);
        return Result.Ok(entity.ToChangeResponse());
    }
}

public class DeleteLimiteHandler : IDeleteLimiteHandler
{
    private readonly ILimiteService _limiteService;

    public DeleteLimiteHandler(ILimiteService limiteService)
    {
        _limiteService = limiteService;
    }

    public async Task<Result> Handle(DeleteLimiteCommand command, CancellationToken cancellationToken = default)
    {
        await _limiteService.Delete(command.Documento, command.Conta, cancellationToken);
        return Result.Ok();
    }
}