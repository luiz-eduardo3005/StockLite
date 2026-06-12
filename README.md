# 📦 StockLite — API REST para Controle de Estoque e Logística

API RESTful desenvolvida em **ASP.NET Core (.NET 8)** para gerenciamento de estoque de produtos, com autenticação JWT e testes unitários.

## 🚀 Tecnologias Utilizadas

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (SQLite + Code-First)
- **JWT** (Autenticação com JSON Web Tokens)
- **xUnit** (Testes Unitários)
- **LINQ** (Consultas no banco de dados)
- **POO + SOLID** (Injeção de Dependência, Interfaces, SRP)

## 📂 Estrutura do Projeto

```
StockLite/
├── StockLite.sln                    # Solução do .NET
│
├── StockLite.Api/                   # Projeto principal (API)
│   ├── Controllers/
│   │   ├── AuthController.cs        # Endpoint de login (JWT)
│   │   └── ProdutosController.cs    # CRUD de produtos
│   ├── Data/
│   │   └── AppDbContext.cs          # Configuração do Entity Framework
│   ├── DTOs/
│   │   ├── LoginRequest.cs          # Objeto de requisição de login
│   │   └── LoginResponse.cs         # Objeto de resposta do login
│   ├── Models/
│   │   ├── Usuario.cs               # Entidade de usuário
│   │   └── Produto.cs               # Entidade de produto (com regras de negócio)
│   ├── Services/
│   │   ├── ITokenService.cs         # Interface do serviço de token
│   │   ├── TokenService.cs          # Geração de tokens JWT
│   │   ├── IProdutoService.cs       # Interface do serviço de produtos
│   │   └── ProdutoService.cs        # Lógica de negócio de produtos
│   ├── Program.cs                   # Ponto de entrada da aplicação
│   └── appsettings.json             # Configurações (JWT, etc.)
│
└── StockLite.Tests/                 # Projeto de testes
    └── ProdutoTests.cs              # Testes unitários do Produto
```

## ⚡ Como Rodar o Projeto

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado

### 1. Clonar e Restaurar
```bash
cd StockLite
dotnet restore
```

### 2. Rodar a API
```bash
dotnet run --project StockLite.Api
```
A API estará disponível em: `http://localhost:5164`

### 3. Rodar os Testes
```bash
dotnet test
```

## 📡 Endpoints da API

### 🔓 Públicos (sem autenticação)

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/auth/login` | Faz login e retorna um token JWT |
| `GET` | `/api/produtos` | Lista todos os produtos |
| `GET` | `/api/produtos/{id}` | Busca um produto por ID |
| `GET` | `/api/produtos/estoque-baixo` | Lista produtos com estoque < 10 |

### 🔒 Protegidos (requerem token JWT)

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/produtos` | Cadastra um novo produto |
| `PUT` | `/api/produtos/{id}` | Atualiza um produto |
| `DELETE` | `/api/produtos/{id}` | Remove um produto |

## 🧪 Testando com curl

### 1. Fazer Login (obter token)
```bash
curl -X POST http://localhost:5164/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@stocklite.com", "senha": "123456"}'
```

### 2. Listar Produtos
```bash
curl http://localhost:5164/api/produtos
```

### 3. Ver Estoque Baixo
```bash
curl http://localhost:5164/api/produtos/estoque-baixo
```

## 📚 Conceitos Demonstrados

- **POO:** Classes, encapsulamento, métodos de validação
- **SOLID (S):** Cada classe tem uma única responsabilidade
- **SOLID (D):** Controllers dependem de interfaces, não de classes concretas
- **Injeção de Dependência:** Nativa do ASP.NET Core
- **Entity Framework Core:** Code-First com SQLite
- **LINQ:** Consultas com `Where()`, `OrderBy()`, `FirstOrDefaultAsync()`
- **JWT:** Geração e validação de tokens para autenticação
- **xUnit:** Testes unitários com padrão AAA (Arrange, Act, Assert)

## 👤 Usuário Padrão (Seed Data)

| Campo | Valor |
|-------|-------|
| Email | `admin@stocklite.com` |
| Senha | `123456` |

## 📄 Licença

Este projeto é para fins educacionais e de portfólio.
