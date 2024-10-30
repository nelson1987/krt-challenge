using Domain.Helpers;
using Domain.Services;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DeleteLimite;

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