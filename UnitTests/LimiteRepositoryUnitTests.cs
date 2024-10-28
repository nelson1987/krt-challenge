using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;
using Moq;
using System.Net;

namespace UnitTests;

public class LimiteRepositoryUnitTests
{
    private readonly LimiteRepository _sut;
    public Mock<IAmazonDynamoDB> dynamoDbMock { get; set; }

    public LimiteRepositoryUnitTests()
    {
        dynamoDbMock = new Mock<IAmazonDynamoDB>();
        _sut = new LimiteRepository(dynamoDbMock.Object);
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        dynamoDbMock.Setup(client => client.PutItemAsync(
               It.IsAny<PutItemRequest>(),
               It.IsAny<CancellationToken>()))
               .ReturnsAsync((PutItemRequest r, CancellationToken token) =>
               {
                   return new PutItemResponse { HttpStatusCode = HttpStatusCode.OK };
               });
        var limite = new Limite("Documento", "Agencia", "Conta", 0.00M);

        limite = await _sut.Incluir(limite);

        Assert.NotNull(limite);
        Assert.NotEqual(Guid.Empty, limite.Id);
        Assert.Equal("Documento", limite.Documento);
        Assert.Equal("Agencia", limite.Agencia);
        Assert.Equal("Conta", limite.Conta);
        Assert.Equal(0.00M, limite.Valor);
    }

    [Fact]
    public async Task IncluirLimite_ProblemaContexto_DisparaExcecaoContexto()
    {
        dynamoDbMock.Setup(client => client.PutItemAsync(
               It.IsAny<PutItemRequest>(),
               It.IsAny<CancellationToken>()))
               .ReturnsAsync((PutItemRequest r, CancellationToken token) =>
               {
                   return new PutItemResponse { HttpStatusCode = HttpStatusCode.BadRequest };
               });
        var limite = new Limite("Documento", "Agencia", "Conta", 0.00M);

        await Assert.ThrowsAsync<ContextoException>(() => _sut.Incluir(limite));
    }
}