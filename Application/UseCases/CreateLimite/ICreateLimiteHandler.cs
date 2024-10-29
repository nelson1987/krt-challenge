using FluentResults;

namespace Application.UseCases.CreateLimite;

public interface ICreateLimiteHandler
{
    Task<Result<CreateLimiteResponse>> Handle(CreateLimiteCommand command, CancellationToken cancellationToken = default);
}

public interface IChangeLimiteHandler
{
    Task<Result<ChangeLimiteResponse>> Handle(ChangeLimiteCommand command, CancellationToken cancellationToken = default);
}

public interface IDeleteLimiteHandler
{
    Task<Result> Handle(DeleteLimiteCommand command, CancellationToken cancellationToken = default);
}