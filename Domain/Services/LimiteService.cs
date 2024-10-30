using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Domain.Services;

public class LimiteService(ILogger<LimiteService> logger, ILimiteRepository _limiteRepository) : ILimiteService
{
    public async Task<Limite> CreateAsync(Limite limite, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(CreateAsync)}");
        var limiteExistente = await _limiteRepository.GetAsync(limite.Documento, limite.Conta, cancellationToken);
        if (limiteExistente != null)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(CreateAsync)}");
            throw new BusinessException(Mensagem.LIMITE_CADASTRADO);
        }
        await _limiteRepository.InsertAsync(limite.ToDto(), cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(CreateAsync)}");
        return limite;
    }

    public async Task DeleteAsync(string documento, string conta, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(DeleteAsync)}");
        await _limiteRepository.DeleteAsync(documento, conta, cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(DeleteAsync)}");
    }

    public async Task<Limite> UpdateAsync(Limite limite, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(DeleteAsync)}");
        var limiteExistente = await _limiteRepository.GetAsync(limite.Documento, limite.Conta, cancellationToken);
        if (limiteExistente == null)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(DeleteAsync)}");
            throw new BusinessException(Mensagem.LIMITE_NAO_CADASTRADO);
        }
        await _limiteRepository.UpdateAsync(limite.ToDto(), cancellationToken);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(DeleteAsync)}");
        return limite;
    }
}