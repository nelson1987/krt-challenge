using FluentValidation;

namespace Application.UseCases.DeleteLimite;

public class DeleteLimiteValidator : AbstractValidator<DeleteLimiteCommand>
{
    public DeleteLimiteValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty();
        RuleFor(x => x.Conta)
            .NotEmpty();
    }
}