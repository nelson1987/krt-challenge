using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.DTOs;
using Domain.Exceptions;
using Domain.Repositories;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class LimiteRepository : ILimiteRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public LimiteRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<LimiteDto?> Buscar(string documento, string agencia, string conta, CancellationToken cancellationToken = default)
    {
        var request = new GetItemRequest
        {
            TableName = "bounds",
            Key = new Dictionary<string, AttributeValue>
            {
                { "document", new AttributeValue { S = documento } },
                { "account", new AttributeValue { S = conta } }
            }
        };

        var response = await _dynamoDb.GetItemAsync(request, cancellationToken);
        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<LimiteDto?>(itemAsDocument.ToJson());
    }

    public async Task<LimiteDto> Incluir(LimiteDto limite, CancellationToken cancellationToken = default)
    {
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = "bounds",
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao incluir limite");
        return limite;
    }
}