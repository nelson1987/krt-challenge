# krt-challenge
# Casos de Uso
## 1. Gest�o de Limites
### WebApi utilizando Repository Methods com conex�o com DynamoDB

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

## 2. Gest�o de Transfer�ncias

## Realizar Transfer�ncia
### [POST] http://localhost:8081/Transferencias
### Endpoint com resili�ncia utilizando a Lib Polly