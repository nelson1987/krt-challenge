using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Repositories;

namespace Domain.Services;

public class LimiteService : ILimiteService
{
    private readonly ILimiteRepository _limiteRepository;

    public LimiteService(ILimiteRepository limiteRepository)
    {
        _limiteRepository = limiteRepository;
    }

    public async Task<Limite> Create(Limite limite, CancellationToken cancellationToken = default)
    {
        var limiteExistente = await _limiteRepository.GetAsync(limite.Documento, limite.Agencia, limite.Conta, cancellationToken);
        if (limiteExistente != null)
            throw new BusinessException(Mensagem.LIMITE_CADASTRADO);
        await _limiteRepository.InsertAsync(limite.ToDto(), cancellationToken);
        return limite;
    }

    public async Task Delete(string documento, string conta, CancellationToken cancellationToken = default)
    {
        //var limiteExistente = await _limiteRepository.GetAsync(documento, limite.Agencia, conta, cancellationToken);
        //if (limiteExistente == null)
        //    throw new BusinessException(Mensagem.LIMITE_NAO_DELETADO);
        await _limiteRepository.DeleteAsync(documento, conta, cancellationToken);
    }

    public async Task<Limite> Update(Limite limite, CancellationToken cancellationToken = default)
    {
        var limiteExistente = await _limiteRepository.GetAsync(limite.Documento, limite.Agencia, limite.Conta, cancellationToken);
        if (limiteExistente == null)
            throw new BusinessException(Mensagem.LIMITE_NAO_CADASTRADO);
        await _limiteRepository.UpdateAsync(limite.ToDto(), cancellationToken);
        return limite;
    }
}