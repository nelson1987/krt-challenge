using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Domain.Entities;
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

    public async Task<Limite> Buscar(string documento, string agencia, string conta, CancellationToken cancellationToken = default)
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
        var dto = JsonSerializer.Deserialize<LimiteDto>(itemAsDocument.ToJson());
        return new Limite(dto.Document, dto.Branch, dto.Account, dto.Value, Guid.Parse(dto.Id));
    }

    //public Task<Limite> BuscarPorId(Guid id)
    //{
    //    var request = new GetItemRequest
    //    {
    //        TableName = "limite",
    //        Key = new Dictionary<string, AttributeValue>
    //        {
    //            { "pk", new AttributeValue { S = id.ToString() } },
    //            { "sk", new AttributeValue { S = id.ToString() } }
    //        }
    //    };

    // var response = await _dynamoDb.GetItemAsync(request); if (response.Item.Count == 0) { return
    // null; }

    //    var itemAsDocument = Document.FromAttributeMap(response.Item);
    //    return JsonSerializer.Deserialize<BoundDto>(itemAsDocument.ToJson());
    //}

    public async Task<Limite> Incluir(Limite limite)
    {
        var dto = new LimiteDto
        {
            Document = limite.Documento,
            Account = limite.Conta,
            Branch = limite.Agencia,
            Value = limite.Valor
        };
        var customerAsJson = JsonSerializer.Serialize(dto);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest
        {
            TableName = "bounds",
            Item = itemAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new ContextoException("Erro ao incluir limite");
        return limite;
    }
}