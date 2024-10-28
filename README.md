# krt-challenge
# Casos de Uso
## 1. Gestão de Limites
### WebApi utilizando Repository Methods com conexão com DynamoDB

## Obter Limite
### [GET] http://localhost:8080/api/Limits/

## Criar Limite
### [POST] http://localhost:8080/api/Limits
### {
###   "document": "string",
###   "branch": "string",
###   "account": "string",
###   "valor": "2024-10-28T14:33:40.850Z"
### }

## Alterar Limite

## Remover Limite

## 2. Gestão de Transferências

## Realizar Transferência
### [POST] http://localhost:8081/Transferencias
### Endpoint com resiliência utilizando a Lib Polly