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

    public async Task Create(string documento, string agencia, string conta, decimal valor)
    {
        //Verifica se existe, se existir retornar um erro
        var limiteExistente = await _limiteRepository.Buscar(documento, agencia, conta);
        if (limiteExistente != null)
            throw new BusinessException(Mensagem.LIMITE_CADASTRADO);
        //Se não existir, criar
        var limite = new Limite(documento, agencia, conta, valor);
        await _limiteRepository.Incluir(limite);
    }

    public async Task<Limite> Create(Limite limite, CancellationToken cancellationToken = default)
    {
        var limiteExistente = await _limiteRepository.Buscar(limite.Documento, limite.Agencia, limite.Conta, cancellationToken);
        if (limiteExistente != null)
            throw new BusinessException(Mensagem.LIMITE_CADASTRADO);
        await _limiteRepository.Incluir(limite);
        return limite;
    }
}