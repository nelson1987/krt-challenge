using FluentResults;

namespace Application.UseCases.DeleteLimite;

public interface IDeleteLimiteHandler
{
    Task<Result> HandleAsync(DeleteLimiteCommand command, CancellationToken cancellationToken = default);
}