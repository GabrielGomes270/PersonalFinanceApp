## 📌 README.md

# 💰 Personal Finance API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-5C2D91)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?logo=docker&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT-orange)
![License](https://img.shields.io/badge/License-MIT-green)

API RESTful para **controle financeiro pessoal**, desenvolvida em **ASP.NET Core 8**, com **MySQL**, **Docker**, **JWT Authentication** e arquitetura baseada em **boas práticas de mercado**.

Projeto desenvolvido com foco em **aprendizado**, **organização**, **segurança** e **portfólio profissional**, simulando comportamentos reais de uma aplicação back-end moderna.

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- MySQL
- Docker & Docker Compose
- JWT (JSON Web Token)
- FluentValidation
- QuestPDF (geração de PDF)
- Swagger / OpenAPI

---

## 📐 Arquitetura do Projeto
```
PersonalFinanceApp
│
├── Controllers
│ ├── AuthController
│ ├── CategoriesController
│ ├── ExpensesController
│ └── ReportExportController
│
├── DTOs
│ ├── Auth
│ ├── Categories
│ ├── Expenses
│ └── Summaries
│
├── Domain
│ └── Entities
│
├── Repositories
│ ├── Interfaces
│ └── Implementations
│
├── Services
│ ├── TokenService
│ └── ReportExportService
│
├── Validators
│
├── Middlewares
│ └── ExceptionHandlingMiddleware
│
├── Data
│ └── AppDbContext
│
└── Program.cs
````
---

## 🔐 Autenticação e Autorização

A API utiliza **JWT** para autenticação de usuários.

### Proteção de endpoints

Endpoints protegidos utilizam:

```csharp
[Authorize]
```
### Somente usuários autenticados podem acessar recursos como:

- Categorias

- Despesas

- Relatórios

- Exportações
### <br>🔑 JWT – Configuração Segura

As configurações sensíveis não são versionadas.

Variáveis de ambiente necessárias:
```
JwtSettings__Key=YOUR_SECRET_KEY
JwtSettings__Issuer=PersonalFinance
JwtSettings__Audience=PersonalFinanceUsers
```

✔ Chave JWT fora do código

✔ Uso de .env e variáveis de ambiente

✔ Preparado para ambientes Docker

### <br> 👤 Funcionalidades Implementadas

### 👥 Usuários

- Registro

- Login

- Autenticação via JWT

### 🏷️ Categorias

- CRUD completo

- Validação com FluentValidation

### 💸 Despesas

- Cadastro, listagem, edição e remoção

- Filtro por categoria

- Filtro por período

- Paginação

- Ordenação dinâmica

---

## 📊 Resumos Financeiros

### 📅 Resumo Mensal

- Total gasto no mês

- Total por categoria

- Exportação em CSV

- Exportação em PDF

### 📆 Resumo Anual

- Total gasto no ano

- Total por mês

- Exportação em CSV

- Exportação em PDF

---

## 📄 Exportação de Relatórios

### CSV

 - Estrutura simples e compatível com Excel e Google Sheets

 - Ideal para análise de dados

### PDF

- Layout formatado com tabelas

- Totalização automática

- Geração dinâmica (não persistida no banco)

---

## 📄 Swagger
A API possui documentação automática via **Swagger**.

📍 Acesse:
```
http://localhost:8080/swagger
```
### Autorização no Swagger

1. Faça login

2. Copie o token JWT

3. Clique em Authorize

4. Use:
```
Bearer {seu_token}
```
---

## 🐳 Docker
O projeto utiliza **Docker Compose** para subir a API e o banco MySQL.

### Subir a aplicação
```
docker compose up -d --build
```
### Derrubar containers
```
docker compose down
```
### Derrubar containers e volumes (apaga dados)
```
docker compose down -v
```
### Serviços

- API: http://localhost:8080

- MySQL: containerizado com volume persistente

---

## 🧪 Validações

Utiliza **FluentValidation** para:

- DTOs de criação e atualização

- Mensagens claras de erro

- Separação de responsabilidades

---

## ⚠️ Middleware Global de Erros
Middleware responsável por:

- Capturar exceções não tratadas

- Retornar respostas JSON padronizadas

- Evitar vazamento de stack trace

### Exemplo de resposta
```
{
  "message": "Ocorreu um erro inesperado no servidor.",
  "status": 500
}
```
---

## 📌 Boas Práticas Aplicadas

- DTOs para evitar vazamento de entidades

- Repository Pattern

- Separação de camadas

- JWT seguro

- Variáveis sensíveis fora do código

- Dockerização

- Código organizado e escalável

---

## 📚 Próximos Passos (Opcional)

- Rate limiting

- Testes automatizados

- Padronização de respostas (Envelope)

- Evolução para gráficos e dashboards

*Itens não implementados para manter a complexidade adequada ao escopo de portfólio.*

## 🧑‍💻 Autor

### **Gabriel Gomes**

Foco em Back-end com .NET
