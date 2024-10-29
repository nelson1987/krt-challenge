using Domain.Entities;

namespace Domain.Repositories;

public interface ILimiteRepository
{
    Task<Limite> Incluir(Limite limite);

    Task<Limite?> Buscar(string documento, string agencia, string conta, CancellationToken cancellationToken = default);
}