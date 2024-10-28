using Domain.Entities;

namespace Application;

public static class CreateLimiteCommandExtensions
{
    public static Limite ToEntity(this CreateLimiteCommand command)
    {
        return new Limite(command.Documento, command.Agencia, command.Conta, command.Valor);
    }

    public static CreateLimiteResponse ToResponse(this Limite entity)
    {
        return new CreateLimiteResponse(entity.Id, entity.Documento, entity.Agencia, entity.Conta, entity.Valor);
    }
}