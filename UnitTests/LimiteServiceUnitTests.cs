using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using Moq;

namespace UnitTests;

public class LimiteServiceUnitTests
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true
        });

    private readonly LimiteService _sut;

    public LimiteServiceUnitTests()
    {
        //var limite = _fixture.Create<Limite>();
        _fixture.Freeze<Mock<ILimiteRepository>>()
        .Setup(client => client.Buscar(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()))
        .ReturnsAsync((Limite?)null);

        _sut = _fixture.Create<LimiteService>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        var documento = "Documento";
        var agencia = "Agencia";
        var conta = "Conta";
        var valor = 0.00M;

        await _sut.Create(documento, agencia, conta, valor);

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.Buscar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.Incluir(It.IsAny<Limite>())
                    , Times.Once);
    }

    [Fact]
    public async Task IncluirLimite_QuandoLimiteExistente_DisparaExcecao()
    {
        var documento = "Documento";
        var agencia = "Agencia";
        var conta = "Conta";
        var valor = 0.00M;

        _fixture.Freeze<Mock<ILimiteRepository>>()
            .Setup(client => client.Buscar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<Limite>());

        await Assert.ThrowsAsync<BusinessException>(() => _sut.Create(documento, agencia, conta, valor));

        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                x.Buscar(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                    , Times.Once);
        _fixture.Freeze<Mock<ILimiteRepository>>()
                .Verify(x =>
                    x.Incluir(It.IsAny<Limite>())
                    , Times.Never);
    }
}