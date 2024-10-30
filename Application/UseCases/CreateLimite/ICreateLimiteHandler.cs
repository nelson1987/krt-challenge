using FluentResults;

namespace Application.UseCases.CreateLimite;

public interface ICreateLimiteHandler
{
    Task<Result<CreateLimiteResponse>> HandleAsync(CreateLimiteCommand command, CancellationToken cancellationToken = default);
}

public interface IChangeLimiteHandler
{
    Task<Result<ChangeLimiteResponse>> HandleAsync(ChangeLimiteCommand command, CancellationToken cancellationToken = default);
}

public interface IDeleteLimiteHandler
{
    Task<Result> HandleAsync(DeleteLimiteCommand command, CancellationToken cancellationToken = default);
}