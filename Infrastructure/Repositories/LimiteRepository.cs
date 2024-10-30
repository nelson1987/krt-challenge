using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.DTOs;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class LimiteRepository(IAmazonDynamoDB _dynamoDb, ILogger<LimiteRepository> logger) : ILimiteRepository
{
    public async Task<LimiteDto?> GetAsync(string documento, string conta, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(GetAsync)}");
        var request = new GetItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Key = new Dictionary<string, AttributeValue>
            {
                { "document", new AttributeValue { S = documento } },
                { "account", new AttributeValue { S = conta } }
            }
        };

        logger.LogInformation($"Message:Iniciado | Method: {nameof(_dynamoDb.GetItemAsync)} | Request: {request.ToJson()}");
        var response = await _dynamoDb.GetItemAsync(request, cancellationToken);
        if (response.Item.Count == 0)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(_dynamoDb.GetItemAsync)} | Request: {response.ToJson()}");
            return null;
        }
        logger.LogInformation($"Message:Finalizado | Method: {nameof(_dynamoDb.GetItemAsync)} | Request: {response.ToJson()}");
        var itemAsDocument = Document.FromAttributeMap(response.Item);
        logger.LogInformation($"Message:Finalizado | Method: {nameof(GetAsync)}");
        return JsonSerializer.Deserialize<LimiteDto?>(itemAsDocument.ToJson());
    }

    public async Task<LimiteDto> UpdateAsync(LimiteDto limite, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(UpdateAsync)}");
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Item = itemAsAttributes
        };

        logger.LogInformation($"Message:Iniciado | Method: {nameof(_dynamoDb.PutItemAsync)} | Request: {updateItemRequest.ToJson()}");
        var response = await _dynamoDb.PutItemAsync(updateItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(_dynamoDb.PutItemAsync)} | Request: {response.ToJson()}");
            throw new ContextoException("Erro ao alterar limite");
        }
        logger.LogInformation($"Message:Finalizado | Method: {nameof(UpdateAsync)}");
        return limite;
    }

    public async Task DeleteAsync(string documento, string conta, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(DeleteAsync)}");
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Key = new Dictionary<string, AttributeValue>
            {
                { "document", new AttributeValue { S = documento } },
                { "account", new AttributeValue { S = conta } }
            }
        };

        logger.LogInformation($"Message:Iniciado | Method: {nameof(_dynamoDb.DeleteItemAsync)} | Request: {deleteItemRequest.ToJson()}");
        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(_dynamoDb.DeleteItemAsync)} | Request: {response.ToJson()}");
            throw new ContextoException("Erro ao deletar limite");
        }
        logger.LogInformation($"Message:Finalizado | Method: {nameof(DeleteAsync)}");
    }

    public async Task<LimiteDto> InsertAsync(LimiteDto limite, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Message:Iniciado | Method: {nameof(InsertAsync)}");
        var customerAsJson = JsonSerializer.Serialize(limite);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = Mensagem.TABELA_BOUNDS,
            Item = itemAsAttributes
        };

        logger.LogInformation($"Message:Iniciado | Method: {nameof(_dynamoDb.PutItemAsync)} | Request: {createItemRequest.ToJson()}");
        var response = await _dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogInformation($"Message:Finalizado | Method: {nameof(_dynamoDb.PutItemAsync)} | Request: {response.ToJson()}");
            throw new ContextoException("Erro ao incluir limite");
        }
        logger.LogInformation($"Message:Finalizado | Method: {nameof(InsertAsync)}");
        return limite;
    }
}