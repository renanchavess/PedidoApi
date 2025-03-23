# Controle de acesso de API de integração

## Contexto 
A empresa REV, especializada na revenda de materiais, identificou a necessidade de otimizar a integração de seus sistemas internos com plataformas externas. Para isso, contratou um desenvolvedor para criar uma API de integração que permita a consulta e o cadastro de clientes e pedidos, além de um painel administrativo para controle de acesso à API.

## Requisitos

### API de integração
[-] O acesso à API deve ser controlado por um token gerado no painel administrativo.
[-] A utilização do token de acesso deve seguir o padrão `Bearer Token`.
[x] A API deve permitir consulta e cadastro de clientes e pedidos.
[x] O formato das respostas deve seguir o padrão JSON.
[x] A API deve permitir atualizar apenas o status do pedido.
[x] A API deve permitir filtrar os clientes fornecendo filtros.
[-] A API deve permitir filtrar os pedidos fornecendo filtros.
[x] A API deve permitir filtrar os produtos fornecendo filtros.(POSSIBILIDADE DE FILTRAR POR QUANTIDADE NO ESTOQUE E PREÇO)
[-] A API deve permitir paginação dos clientes.
[-] A API deve permitir paginação dos pedidos.
[-] A API deve permitir paginação dos produtos.

### Painel Administrativo
- O painel deve possuir login utilizando `BASIC AUTH`.
- O painel deve permitir geração de tokens de acesso.
- O painel deve permitir revogação de tokens de acesso.

### Contratos
#### Clientes
``` json
// Contrato de cadastro/atualização de clientes
// REQUEST
{
  "nome": "string",
  "email": "string",
  "telefone": "string",
  "endereco": {
    "rua": "string",
    "numero": "string",
    "cidade": "string",
    "estado": "string",
    "cep": "string"
  }
}
// RESPONSE
{
  "id": 0,
  "nome": "string",
  "status": "Ativo"
}
```
``` json
// Contrato de consulta de clientes
[
  {
    "id": 0,
    "nome": "string",
    "email": "string",
    "telefone": "string",
    "status": "Ativo"
  }
]
```

#### Pedidos
``` json
// Contratos de cadastro/atualização de pedidos
// REQUEST
{
  "cliente_id": 0,
  "itens": [
    {
      "produto_id": 0,
      "quantidade": 0,
      "preco_unitario": 0.0
    }
  ]
}
// RESPONSE
{
  "pedido_id": 0,
  "cliente_id": 0,
  "status": "Em Processamento"
}
```
``` json
// Contrato de consulta de pedidos
[
  {
    "pedido_id": 0,
    "cliente_id": 0,
    "itens": [
      {
        "produto_id": 0,
        "quantidade": 0,
        "preco_unitario": 0.0
      }
    ],
    "valor_total": 0.0,
    "status": "Em Processamento"
  }
]
```
#### TOKENS
``` json
// Contrato de cadastro de tokens
// REQUEST
{
  "descricao": "string",
  "expiracao": "2025-12-31T23:59:59Z"
}
// RESPONSE
{
  "token": "string"
}
```
``` json
// Contrato de consulta de tokens
[
  {
    "descricao": "string",
    "expiracao": "2025-12-31T23:59:59Z"
  }
]
```
#### Produtos
``` json
// Contrato de consulta de produtos
[
  {
    "id": 0,
    "nome": "string",
    "precoUnitario": 0.0,
    "quantidadeEstoque": 0.0
  }
]
```

## Scripts auxiliares
``` sql
INSERT INTO produtos 
(id, nome, precoUnitario, quantidadeEstoque) 
VALUES
(1, 'Cimento CP-II 50kg', 35.90, 100),
(2, 'Tijolo Cerâmico 9x14x19cm', 1.20, 5000),
(3, 'Areia Média 20kg', 12.50, 200),
(4, 'Brita 1 20kg', 14.00, 150),
(5, 'Viga de Aço 6m', 120.00, 50),
(6, 'Telha de Fibrocimento 2,44x1,10m', 45.00, 75),
(7, 'Tinta Acrílica Branca 18L', 230.00, 30),
(8, 'Pincel 2 Polegadas', 15.50, 100),
(9, 'Massa Corrida 25kg', 65.00, 80),
(10, 'Chave de Fenda 1/4x6"', 22.90, 120);

```

### Entregas
Deve ser entregue o link do github via e-mail (`leandro.santos@movtech.com.br`).
<br>
**No repositório deve conter:**
- Projeto frontend/backend
- Scripts de migração inicial do banco de dados

### Regras
- Não utilizar ORM ou MicroORM
- Pode ser utilizado qualquer banco relacional de sua preferência
- O backend deve ser escrito em .Net
- O frontend deve ser escrito em Angular

## Diferenciais (opcional) 
- Dockerizar a aplicação
- Teste unitário utilizando XUnit
- Registrar log para auditoria
- Restringir acesso do token por IP