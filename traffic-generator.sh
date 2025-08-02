#!/bin/bash

# Faz a autenticação uma vez e captura o token
echo "Autenticando..."
token=$(curl --silent --location 'http://localhost:8080/api/Auth/login' \
  --header 'Content-Type: application/json' \
  --header 'Accept: application/json' \
  --data-raw '{
    "email": "teste@teste.com",
    "password": "teste@123"
  }' | jq -r '.token')

if [ -z "$token" ] || [ "$token" == "null" ]; then
  echo "Erro ao obter token. Abortando."
  exit 1
fi

echo "Token obtido: $token"

for i in {1..10000}
do
  # Requests simples sem auth (health e fiap-dotnet)
  curl -s -k -H 'header info' -b 'ff' "http://localhost:8080/health"
  curl -s -k -H 'header info' -b 'ff' "http://localhost:8080/fiap-dotnet"

  # Requests autenticados com o token obtido
  curl --silent --location "http://localhost:8080/api/Games" \
    --header "Content-Type: application/json" \
    --header "Authorization: Bearer $token" \
    --data '{"categoryId":"016b7b0b-afee-4492-9b89-e26476ed1f31","name":"jogo teste","price":50,"description":"descrição teste","imageUrl":""}'

  curl --silent --location "http://localhost:8080/api/games/simulate-db-error" \
    --header "Authorization: Bearer $token"

  curl --silent --location "http://localhost:8080/api/User" \
    --header "Authorization: Bearer $token"

  curl --silent --location "http://localhost:8080/api/Games" \
    --header "Authorization: Bearer $token"

done

echo "Requisições finalizadas."
