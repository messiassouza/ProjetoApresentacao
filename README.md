# ProjetoApresentacao 

Este projeto consiste em uma API RESTful desenvolvida em ASP.NET Core, que permite a gestão de um catálogo de produtos. A API inclui operações CRUD (Create, Read, Update, Delete) para os produtos, utilizando SQL Server como mecanismo de persistência de dados.

## Funcionalidades

- **Cadastro de Produto:** 
  - O produto contém as seguintes propriedades:
    - `Id` (int, chave primária)
    - `Nome` (string, obrigatório)
    - `Descrição` (string, opcional)
    - `Preço` (decimal, obrigatório)
    - `Data de criação` (DateTime, obrigatório)

- **Listar Produtos:** 
  - A API retorna uma lista de produtos com paginação.

- **Atualizar Produto:** 
  - Permite a atualização de qualquer campo do produto existente.

- **Deletar Produto:** 
  - Implementa a funcionalidade de exclusão lógica (soft delete), onde o produto não é removido do banco de dados, mas marcado como excluído.

- **Buscar Produto por ID:** 
  - A API retorna um produto específico com base no ID.

## Requisitos Técnicos

- Utiliza .NET 6 ou superior.
- Utiliza Entity Framework Core como ORM.
- O banco de dados é criado utilizando migrações do Entity Framework.
- Segue as melhores práticas de Clean Code e SOLID na organização do código.
- Utiliza o padrão Repository para acesso a dados.
- Implementa o Swagger para documentar os endpoints da API.
- Desenvolve testes unitários utilizando xUnit, NUnit ou similar.

## Requisitos Adicionais (Diferenciais)

- Autenticação e autorização utilizando JWT.
- Implementação de cache para consultas de listagem de produtos.

## Como Configurar e Rodar o Projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu_usuario/ProjetoApresentacao.git


Acesse a API em http://localhost:5022.

Para visualizar a documentação dos endpoints, acesse  http://localhost:5022/swagger (EXEMPLO).

Tecnologias Utilizadas
ASP.NET Core
Entity Framework Core
SQL Server
JWT para autenticação
Swagger para documentação da API
xUnit para testes unitários
Critérios de Avaliação
Qualidade do código (estrutura, organização e aderência às boas práticas).
Funcionamento correto das funcionalidades solicitadas.
Uso adequado do mecanismo de persistência.
Documentação clara e objetiva neste README.

