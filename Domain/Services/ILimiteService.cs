using Domain.Entities;

namespace Domain.Services;

public interface ILimiteService
{
    Task Create(string documento, string agencia, string conta, decimal valor);

    Task<Limite> Create(Limite limite, CancellationToken cancellationToken = default);
}