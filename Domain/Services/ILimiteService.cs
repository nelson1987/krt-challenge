using Domain.Entities;

namespace Domain.Services;

public interface ILimiteService
{
    Task<Limite> Create(Limite limite, CancellationToken cancellationToken = default);

    Task<Limite> Update(Limite limite, CancellationToken cancellationToken = default);

    Task Delete(string documento, string conta, CancellationToken cancellationToken = default);
}