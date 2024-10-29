using Application.UseCases.CreateLimite;
using AutoFixture;

namespace UnitTests;

public class CreateLimiteHandlerUnitTests : UnitTestsBase
{
    private readonly CreateLimiteHandler _sut;
    private readonly CreateLimiteCommand _command;

    public CreateLimiteHandlerUnitTests()
    {
        _command = _fixture.Create<CreateLimiteCommand>();
        _sut = _fixture.Create<CreateLimiteHandler>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        var handler = await _sut.Handle(_command, CancellationToken.None);

        Assert.True(handler.IsSuccess);
        var response = handler.Value;
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(_command.Documento, response.Documento);
        Assert.Equal(_command.Agencia, response.Agencia);
        Assert.Equal(_command.Conta, response.Conta);
        Assert.Equal(_command.Valor, response.Valor);
    }
}