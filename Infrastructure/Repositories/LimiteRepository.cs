﻿using Amazon.DynamoDBv2;
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

    public Task<Limite> Buscar(string documento, string agencia, string conta)
    {
        throw new NotImplementedException();
    }

    public async Task<Limite> Incluir(Limite limite)
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