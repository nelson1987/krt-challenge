using AutoFixture;
using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using Moq;

namespace UnitTests;

public class LimiteServiceUnitTests : UnitTestsBase
{
    private readonly LimiteService _sut;

    public LimiteServiceUnitTests()
    {
        _fixture.Freeze<Mock<ILimiteRepository>>()
        .Setup(client => client.Buscar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
        .ReturnsAsync((LimiteDto?)null);

        _sut = _fixture.Create<LimiteService>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        var limite = new Limite("Documento", "Agencia", "Conta", 0.01M);

        await _sut.Create(limite, CancellationToken.None);

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.Buscar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.Incluir(It.IsAny<LimiteDto>(), It.IsAny<CancellationToken>())
                    , Times.Once);
    }

    [Fact]
    public async Task IncluirLimite_QuandoLimiteExistente_DisparaExcecao()
    {
        var limite = new Limite("Documento", "Agencia", "Conta", 0.01M);

        _fixture.Freeze<Mock<ILimiteRepository>>()
            .Setup(client => client.Buscar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Create<LimiteDto>());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.Create(limite, CancellationToken.None));

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.Buscar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.Incluir(It.IsAny<LimiteDto>(), It.IsAny<CancellationToken>())
                    , Times.Never);
    }
}