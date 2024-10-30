using Application.UseCases.ChangeLimite;
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