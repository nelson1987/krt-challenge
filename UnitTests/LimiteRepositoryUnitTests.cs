using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Moq;
using System.Net;
using System.Text.Json;

namespace UnitTests;

public class UnitTestsBase
{
    public LimiteRepository contexto { get; set; }
    public Mock<IAmazonDynamoDB> dynamoDbMock { get; set; }

    public UnitTestsBase()
    {
        dynamoDbMock = new Mock<IAmazonDynamoDB>();
        contexto = new LimiteRepository(dynamoDbMock.Object);
    }
}

public class LimiteRepositoryUnitTests : UnitTestsBase
{
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
        limite = await contexto.Incluir(limite);

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
        await Assert.ThrowsAsync<ContextoException>(() => contexto.Incluir(limite));
    }
}

public class ContextoException : Exception
{
    public ContextoException(string message) : base(message)
    {
    }
}

public class LimiteRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public LimiteRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<Limite?> Incluir(Limite limite)
    {
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = "limite",
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao incluir limite");
        return limite;
    }
}

public class Limite
{
    public Limite(string documento, string agencia, string conta, decimal valor)
    {
        Id = Guid.NewGuid();
        Documento = documento;
        Agencia = agencia;
        Conta = conta;
        Valor = valor;
    }

    public Guid Id { get; private set; }
    public string Documento { get; private set; }
    public string Agencia { get; private set; }
    public string Conta { get; private set; }
    public decimal Valor { get; private set; }
}