using Application.UseCases.ChangeLimite;
using Application.UseCases.CreateLimite;
using AutoFixture;
using Domain.Entities;
using Domain.Services;
using Moq;

namespace UnitTests;

public class CreateLimiteHandlerUnitTests : UnitTestsBase
{
    private readonly CreateLimiteHandler _sut;
    private readonly CreateLimiteCommand _command;

    public CreateLimiteHandlerUnitTests()
    {
        _fixture.Freeze<Mock<ILimiteService>>();
        _command = _fixture.Create<CreateLimiteCommand>();
        _sut = _fixture.Create<CreateLimiteHandler>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        _fixture.Freeze<Mock<ILimiteService>>()
             .Setup(x => x.CreateAsync(It.IsAny<Limite>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(_command.ToEntity());

        var handler = await _sut.HandleAsync(_command, CancellationToken.None);

        Assert.True(handler.IsSuccess);
        var response = handler.Value;
        Assert.Equal(_command.Documento, response.Documento);
        Assert.Equal(_command.Agencia, response.Agencia);
        Assert.Equal(_command.Conta, response.Conta);
        Assert.Equal(_command.Valor, response.Valor);
    }
}