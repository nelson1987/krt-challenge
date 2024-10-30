using Domain.Helpers;
using Domain.Services;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateLimite;

public class CreateLimiteHandler(ILogger<CreateLimiteHandler> logger, ILimiteService _limiteService) : ICreateLimiteHandler
{
    public async Task<Result<CreateLimiteResponse>> HandleAsync(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(HandleAsync)}");
        var limite = command.ToEntity();
        logger.LogInformation($"Message:Iniciado | Method: {nameof(_limiteService.CreateAsync)} | Request: {limite.ToJson()}");
        var entity = await _limiteService.CreateAsync(limite, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(_limiteService.CreateAsync)} | Request: {entity.ToJson()}");
        logger.LogInformation($"Message:Finalizado | Method: {nameof(HandleAsync)}");
        return Result.Ok(entity.ToCreateResponse());
    }
}

public class ChangeLimiteHandler(ILogger<ChangeLimiteHandler> logger, ILimiteService _limiteService) : IChangeLimiteHandler
{
    public async Task<Result<ChangeLimiteResponse>> HandleAsync(ChangeLimiteCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(HandleAsync)}");
        var limite = command.ToEntity();
        logger.LogInformation($"Message:Iniciado | Method: {nameof(_limiteService.UpdateAsync)} | Request: {limite.ToJson()}");
        var entity = await _limiteService.UpdateAsync(limite, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(_limiteService.UpdateAsync)} | Request: {entity.ToJson()}");
        logger.LogInformation($"Message:Finalizado | Method: {nameof(HandleAsync)}");
        return Result.Ok(entity.ToChangeResponse());
    }
}

public class DeleteLimiteHandler(ILogger<DeleteLimiteHandler> logger, ILimiteService _limiteService) : IDeleteLimiteHandler
{
    public async Task<Result> HandleAsync(DeleteLimiteCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(HandleAsync)}");
        logger.LogInformation($"Message:Iniciado | Method: {nameof(_limiteService.DeleteAsync)} | Request: {command.ToJson()}");
        await _limiteService.DeleteAsync(command.Documento, command.Conta, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(_limiteService.DeleteAsync)}");
        logger.LogInformation($"Message:Finalizado | Method: {nameof(HandleAsync)}");
        return Result.Ok();
    }
}