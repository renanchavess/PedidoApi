O backend creio que implementei completo porem do front está listando, criando e revogando os tokens faltou implementar o login apenas.

## Banco de dados
- SqlServer
- Script de criação DatabaseInit.sql

## USUARIO ADMINISTRATIVO
email: admin
senha: password123

### API de integração
- [x] O acesso à API deve ser controlado por um token gerado no painel administrativo.
- [x] A utilização do token de acesso deve seguir o padrão `Bearer Token`.
- [x] A API deve permitir consulta e cadastro de clientes e pedidos.
- [x] O formato das respostas deve seguir o padrão JSON.
- [x] A API deve permitir atualizar apenas o status do pedido.
- [x] A API deve permitir filtrar os clientes fornecendo filtros.
- [x] A API deve permitir filtrar os pedidos fornecendo filtros.
- [x] A API deve permitir filtrar os produtos fornecendo filtros.
- [x] A API deve permitir paginação dos clientes.
- [x] A API deve permitir paginação dos pedidos.
- [x] A API deve permitir paginação dos produtos.

### Painel Administrativo
- [ ] O painel deve possuir login utilizando `BASIC AUTH`.
- [x] O painel deve permitir geração de tokens de acesso.
- [x] O painel deve permitir revogação de tokens de acesso.
