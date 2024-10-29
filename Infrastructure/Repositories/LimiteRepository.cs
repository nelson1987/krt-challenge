using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.DTOs;
using Domain.Exceptions;
using Domain.Helpers;
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

    public async Task<LimiteDto?> GetAsync(string documento, string agencia, string conta, CancellationToken cancellationToken = default)
    {
        var request = new GetItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
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

    public async Task<LimiteDto> UpdateAsync(LimiteDto limite, CancellationToken cancellationToken = default)
    {
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao alterar limite");
        return limite;
    }

    public async Task DeleteAsync(string documento, string conta, CancellationToken cancellationToken = default)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Key = new Dictionary<string, AttributeValue>
            {
                { "document", new AttributeValue { S = documento } },
                { "account", new AttributeValue { S = conta } }
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao deletar limite");
    }

    public async Task<LimiteDto> InsertAsync(LimiteDto limite, CancellationToken cancellationToken = default)
    {
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao incluir limite");
        return limite;
    }
}