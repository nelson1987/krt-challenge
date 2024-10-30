using Application.UseCases.CreateLimite;
using Domain.Helpers;
using Domain.Services;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ChangeLimite;

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
