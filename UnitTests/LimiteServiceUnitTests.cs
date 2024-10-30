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
    private readonly Limite _limite;

    public LimiteServiceUnitTests()
    {
        _fixture.Freeze<Mock<ILimiteRepository>>()
        .Setup(client => client.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
        .ReturnsAsync((LimiteDto?)null);

        _limite = new Limite("Documento", "Agencia", "Conta", 0.01M);
        _sut = _fixture.Create<LimiteService>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        await _sut.CreateAsync(_limite, CancellationToken.None);

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.InsertAsync(It.IsAny<LimiteDto>(), It.IsAny<CancellationToken>())
                    , Times.Once);
    }

    [Fact]
    public async Task IncluirLimite_QuandoLimiteExistente_DisparaExcecao()
    {
        _fixture.Freeze<Mock<ILimiteRepository>>()
            .Setup(client => client.GetAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Create<LimiteDto>());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.CreateAsync(_limite, CancellationToken.None));

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.InsertAsync(It.IsAny<LimiteDto>(), It.IsAny<CancellationToken>())
                    , Times.Never);
    }
}