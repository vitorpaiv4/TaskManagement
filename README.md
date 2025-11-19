# 📦 Sistema de Controle de Estoque

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

> Sistema completo de gerenciamento de produtos e movimentações de estoque desenvolvido com Clean Architecture, princípios SOLID e padrões de projeto.

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Padrões de Projeto](#-padrões-de-projeto)
- [Princípios SOLID](#-princípios-solid)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Execução](#-instalação-e-execução)
- [Uso da API](#-uso-da-api)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Testes](#-testes)
- [Demonstração](#-demonstração)

---

## 🎯 Sobre o Projeto

Este projeto foi desenvolvido como trabalho acadêmico para demonstrar a aplicação prática de **Engenharia de Software** moderna, incluindo:

✅ **Arquitetura em Camadas** (Clean Architecture)  
✅ **Princípios SOLID** (DIP, SRP, OCP)  
✅ **Padrões de Projeto** (Factory Method, Strategy, Repository)  
✅ **Testes Automatizados** (xUnit + Moq)  
✅ **API RESTful** com documentação Swagger  
✅ **Inversão de Dependência** completa entre camadas

O sistema permite gerenciar produtos e suas movimentações de estoque (entradas e saídas), com validação de regras de negócio e rastreabilidade completa.

---

## ⚡ Funcionalidades

### 🛒 Gestão de Produtos
- ✅ Cadastro de produtos com SKU único
- ✅ Consulta de produtos individuais ou listagem completa
- ✅ Controle automático de quantidade em estoque
- ✅ Validação de SKU duplicado

### 📊 Movimentações de Estoque
- ✅ **ENTRADA**: Adiciona quantidade ao estoque
- ✅ **SAÍDA**: Remove quantidade com validação de disponibilidade
- ✅ Histórico completo de movimentações
- ✅ Rastreabilidade (data, quantidade, tipo)
- ✅ Relacionamento com produto

### 🔒 Validações de Negócio
- ✅ Não permite saída maior que estoque disponível
- ✅ SKU único por produto
- ✅ Validação de tipos de movimentação
- ✅ Tratamento de erros com mensagens descritivas

---

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** com separação clara de responsabilidades:

```
┌─────────────────────────────────────────────────┐
│           API Layer (Apresentação)              │
│   • Endpoints REST                              │
│   • Swagger/OpenAPI                             │
│   • Dependency Injection                        │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│      Application Layer (Lógica de Negócio)      │
│   • Services                                    │
│   • Factory Method Pattern                      │
│   • Strategy Pattern                            │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│        Domain Layer (Núcleo do Sistema)         │
│   • Entidades (Product, Movement)               │
│   • Interfaces (Contratos)                      │
│   • Regras de Domínio                           │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│    Infrastructure Layer (Persistência)          │
│   • Entity Framework Core                       │
│   • SQL Server                                  │
│   • Repository Pattern                          │
└─────────────────────────────────────────────────┘
```

### Benefícios da Arquitetura

- 🎯 **Separação de Responsabilidades**: Cada camada tem um propósito único
- 🔄 **Inversão de Dependência**: Camadas dependem de abstrações, não implementações
- 🧪 **Testabilidade**: Fácil criar testes unitários com mocks
- 🔌 **Extensibilidade**: Adicionar novos tipos de movimentação sem alterar código existente
- 🛡️ **Manutenibilidade**: Mudanças isoladas em cada camada

---

## 💻 Tecnologias

### Core
- **[.NET 8.0](https://dotnet.microsoft.com/)** - Framework principal
- **[C# 12](https://docs.microsoft.com/en-us/dotnet/csharp/)** - Linguagem de programação
- **[ASP.NET Core Minimal APIs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)** - Framework web

### Persistência
- **[Entity Framework Core 8.0](https://docs.microsoft.com/en-us/ef/core/)** - ORM
- **[SQL Server](https://www.microsoft.com/sql-server)** - Banco de dados relacional
- **[LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)** - Desenvolvimento local

### Testes
- **[xUnit](https://xunit.net/)** - Framework de testes
- **[Moq](https://github.com/moq/moq4)** - Framework de mocking

### Documentação
- **[Swagger/OpenAPI](https://swagger.io/)** - Documentação interativa da API
- **[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Geração automática de documentação

---

## 🎨 Padrões de Projeto

### 1️⃣ Factory Method Pattern

**Classe**: `MovementFactory`  
**Localização**: `TaskManagement.Application/Factories/`

```csharp
public class MovementFactory : IMovementFactory
{
    public Movement CreateMovement(int productId, int quantity, string type)
    {
        return new Movement
        {
            ProductId = productId,
            Quantity = quantity,
            Type = type,
            Date = DateTime.UtcNow  // ← Regra centralizada
        };
    }
}
```

**Benefícios**:
- ✅ Centraliza a criação de objetos
- ✅ Garante que toda movimentação tenha data UTC
- ✅ Facilita mudanças nas regras de criação
- ✅ **Princípio SOLID**: Single Responsibility Principle (SRP)

---

### 2️⃣ Strategy Pattern

**Classes**: `EntryStrategy`, `ExitStrategy`  
**Localização**: `TaskManagement.Application/Strategies/`

```csharp
public class EntryStrategy : IMovementStrategy
{
    public string TypeHandled => "ENTRADA";

    public void Process(Product product, Movement movement)
    {
        product.StockQuantity += movement.Quantity;
    }
}

public class ExitStrategy : IMovementStrategy
{
    public string TypeHandled => "SAÍDA";

    public void Process(Product product, Movement movement)
    {
        if (product.StockQuantity < movement.Quantity)
            throw new InvalidOperationException("Estoque insuficiente");
            
        product.StockQuantity -= movement.Quantity;
    }
}
```

**Benefícios**:
- ✅ Encapsula lógica específica de cada tipo de movimentação
- ✅ Permite adicionar novos tipos sem modificar código existente
- ✅ **Princípio SOLID**: Open/Closed Principle (OCP)
- ✅ Facilita testes unitários de cada estratégia isoladamente

---

### 3️⃣ Repository Pattern

**Classes**: `ProductRepository`, `MovementRepository`  
**Localização**: `TaskManagement.Infrastructure/Repositories/`

```csharp
public class ProductRepository : IProductRepository
{
    private readonly TaskManagementDbContext _context;

    public Product? GetById(int id)
    {
        return _context.Products.AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }
    // ... outros métodos CRUD
}
```

**Benefícios**:
- ✅ Abstrai o acesso a dados
- ✅ Facilita troca de tecnologia de persistência
- ✅ **Princípio SOLID**: Dependency Inversion Principle (DIP)
- ✅ Permite mockar repositórios em testes

---

## 🎯 Princípios SOLID

### **S**ingle Responsibility Principle (SRP)
Cada classe tem uma única responsabilidade:
- `MovementFactory`: apenas criar movimentações
- `EntryStrategy`: apenas processar entradas
- `ProductRepository`: apenas persistir produtos

### **O**pen/Closed Principle (OCP)
Sistema aberto para extensão, fechado para modificação:
- Adicionar novo tipo de movimentação? Crie uma nova `Strategy`
- Não precisa alterar `MovementService`

### **L**iskov Substitution Principle (LSP)
Implementações podem substituir interfaces sem quebrar o sistema:
- Qualquer `IMovementStrategy` pode ser usada pelo `MovementService`

### **I**nterface Segregation Principle (ISP)
Interfaces específicas e coesas:
- `IProductRepository` com apenas operações de produto
- `IMovementFactory` com apenas criação de movimentações

### **D**ependency Inversion Principle (DIP)
Camadas dependem de abstrações, não implementações concretas:
```csharp
// ✅ Correto: Depende da interface
public MovementService(IMovementRepository repository) { }

// ❌ Errado: Depender da implementação concreta
public MovementService(MovementRepository repository) { }
```

---

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- ✅ [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- ✅ [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (incluído no Visual Studio)
- ✅ [Git](https://git-scm.com/) (para clonar o repositório)

---

## 🚀 Instalação e Execução

### 1️⃣ Clone o Repositório

```bash
git clone https://github.com/vitorpaiv4/TaskManagement.git
cd TaskManagement
```

### 2️⃣ Restaure as Dependências

```bash
dotnet restore
```

### 3️⃣ Configure o Banco de Dados

#### Criar a migração (se ainda não existir):
```bash
dotnet ef migrations add InitialStockSystem --project TaskManagement.Infrastructure --startup-project TaskManagement.API
```

#### Aplicar a migração:
```bash
dotnet ef database update --project TaskManagement.API
```

### 4️⃣ Execute a Aplicação

```bash
dotnet run --project TaskManagement.API
```

Ou pressione **F5** no Visual Studio.

### 5️⃣ Acesse o Swagger

Abra o navegador em:
```
http://localhost:5122/swagger
```

---

## 📡 Uso da API

### Endpoints Disponíveis

#### 🏥 Health Check
```http
GET /health
```

**Resposta**:
```json
{
  "status": "OK",
  "message": "Sistema de Controle de Estoque - API Running"
}
```

---

#### 📦 Listar Todos os Produtos
```http
GET /api/products
```

**Resposta**:
```json
[
  {
    "id": 1,
    "sku": "MOUSE-001",
    "name": "Mouse Gamer RGB",
    "description": "Mouse óptico 16000 DPI",
    "price": 149.90,
    "stockQuantity": 45,
    "createdAt": "2025-11-19T12:00:00Z"
  }
]
```

---

#### 🔍 Buscar Produto por ID
```http
GET /api/products/{id}
```

**Parâmetros**:
- `id` (path): ID do produto

**Resposta**: Mesma estrutura do produto individual

---

#### ➕ Cadastrar Produto
```http
POST /api/products
Content-Type: application/json
```

**Body**:
```json
{
  "sku": "TECLADO-001",
  "name": "Teclado Mecânico",
  "description": "Switch Blue, RGB",
  "price": 299.90
}
```

**Resposta** (201 Created):
```json
{
  "id": 2,
  "sku": "TECLADO-001",
  "name": "Teclado Mecânico",
  "description": "Switch Blue, RGB",
  "price": 299.90,
  "stockQuantity": 0,
  "createdAt": "2025-11-19T12:30:00Z"
}
```

---

#### 📊 Listar Movimentações
```http
GET /api/movements
```

**Resposta**:
```json
[
  {
    "id": 1,
    "productId": 1,
    "quantity": 50,
    "type": "ENTRADA",
    "date": "2025-11-19T12:15:00Z",
    "product": {
      "id": 1,
      "name": "Mouse Gamer RGB",
      "sku": "MOUSE-001"
    }
  }
]
```

---

#### ➕ Registrar Movimentação (ENTRADA)
```http
POST /api/movements
Content-Type: application/json
```

**Body**:
```json
{
  "productId": 1,
  "quantity": 100,
  "type": "ENTRADA"
}
```

**Resposta** (201 Created):
```json
{
  "id": 2,
  "productId": 1,
  "quantity": 100,
  "type": "ENTRADA",
  "date": "2025-11-19T12:45:00Z"
}
```

---

#### ➖ Registrar Movimentação (SAÍDA)
```http
POST /api/movements
Content-Type: application/json
```

**Body**:
```json
{
  "productId": 1,
  "quantity": 10,
  "type": "SAÍDA"
}
```

**Resposta** (201 Created): Mesmo formato da entrada

**Erro** (400 Bad Request - Estoque Insuficiente):
```json
{
  "error": "Estoque insuficiente. Disponível: 5, Solicitado: 10"
}
```

---

## 📂 Estrutura do Projeto

```
TaskManagement/
│
├── TaskManagement.Domain/                 # Camada de Domínio
│   ├── Entities/
│   │   ├── Product.cs                    # Entidade Produto
│   │   └── Movement.cs                   # Entidade Movimentação
│   └── Interfaces/
│       ├── IProductRepository.cs         # Contrato do repositório
│       ├── IMovementRepository.cs
│       ├── IMovementService.cs           # Contrato do serviço
│       ├── IMovementFactory.cs           # Contrato da fábrica
│       └── IMovementStrategy.cs          # Contrato da estratégia
│
├── TaskManagement.Application/            # Camada de Aplicação
│   ├── Services/
│   │   └── MovementService.cs            # Lógica de negócio
│   ├── Factories/
│   │   └── MovementFactory.cs            # Factory Method
│   └── Strategies/
│       ├── EntryStrategy.cs              # Estratégia ENTRADA
│       └── ExitStrategy.cs               # Estratégia SAÍDA
│
├── TaskManagement.Infrastructure/         # Camada de Infraestrutura
│   ├── Data/
│   │   └── TaskManagementDbContext.cs    # Contexto EF Core
│   ├── Repositories/
│   │   ├── ProductRepository.cs          # Implementação do repositório
│   │   └── MovementRepository.cs
│   └── Migrations/                        # Migrações do banco
│
├── TaskManagement.API/                    # Camada de API
│   ├── Program.cs                         # Configuração e endpoints
│   └── appsettings.json                   # Configurações
│
└── TaskManagement.Tests.Unit/             # Testes Unitários
    └── MovementServiceTests.cs            # Testes do serviço
```

---

## 🧪 Testes

O projeto inclui **testes automatizados** usando **xUnit** e **Moq** para garantir qualidade e funcionamento correto das regras de negócio.

### Executar Todos os Testes

```bash
dotnet test
```

### Testes Implementados

#### ✅ Teste 1: Processo de Movimentação Completo
```csharp
[Fact]
public void ProcessNewMovement_ShouldUseFactoryAndStrategy()
```
**Valida**:
- Factory é chamada corretamente
- Strategy é selecionada e executada
- Repositório persiste a movimentação

#### ✅ Teste 2: Validação de Estoque Insuficiente
```csharp
[Fact]
public void ExitStrategy_ShouldFailWhenStockIsZero()
```
**Valida**:
- `ExitStrategy` lança exceção quando estoque é insuficiente
- Mensagem de erro é descritiva

#### ✅ Teste 3: Factory Configura Data e Tipo
```csharp
[Fact]
public void MovementFactory_ShouldSetTypeAndDate()
```
**Valida**:
- Factory configura tipo corretamente
- Data é definida como UTC
- Quantidade é preservada

### Cobertura de Testes

- ✅ Serviços (MovementService)
- ✅ Strategies (EntryStrategy, ExitStrategy)
- ✅ Factories (MovementFactory)
- ⚠️ Repositórios (testes de integração não incluídos)

---

## 🎬 Demonstração

### Fluxo Completo de Uso

#### 1️⃣ Cadastrar um Produto
```bash
POST /api/products
{
  "sku": "HEADSET-001",
  "name": "Headset Gamer 7.1",
  "description": "Som surround, RGB",
  "price": 349.90
}
```

#### 2️⃣ Registrar Entrada de Estoque
```bash
POST /api/movements
{
  "productId": 1,
  "quantity": 50,
  "type": "ENTRADA"
}
```
**Estoque atual**: 50 unidades

#### 3️⃣ Registrar Saída de Estoque
```bash
POST /api/movements
{
  "productId": 1,
  "quantity": 15,
  "type": "SAÍDA"
}
```
**Estoque atual**: 35 unidades (50 - 15)

#### 4️⃣ Tentar Saída Maior que Estoque
```bash
POST /api/movements
{
  "productId": 1,
  "quantity": 100,
  "type": "SAÍDA"
}
```
**Resultado**: ❌ Erro 400 - "Estoque insuficiente. Disponível: 35, Solicitado: 100"

#### 5️⃣ Consultar Histórico
```bash
GET /api/movements
```
Retorna todas as movimentações com detalhes do produto.

---

## 📚 Aprendizados e Conceitos Aplicados

### Engenharia de Software
- ✅ Clean Architecture
- ✅ Separation of Concerns
- ✅ Dependency Injection
- ✅ Inversion of Control (IoC)

### Princípios SOLID
- ✅ Single Responsibility Principle
- ✅ Open/Closed Principle
- ✅ Liskov Substitution Principle
- ✅ Interface Segregation Principle
- ✅ Dependency Inversion Principle

### Padrões de Projeto
- ✅ Factory Method
- ✅ Strategy
- ✅ Repository

### Boas Práticas
- ✅ Testes Automatizados
- ✅ Código Limpo (Clean Code)
- ✅ Documentação de API (Swagger)
- ✅ Versionamento (Git)
- ✅ Tratamento de Erros
- ✅ Validação de Regras de Negócio

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Vitor Paiva**

- GitHub: [@vitorpaiv4](https://github.com/vitorpaiv4)
- LinkedIn: [Vitor Paiva](https://linkedin.com/in/vitorpaiv4)

---

## 🙏 Agradecimentos

- Professores e colegas da faculdade
- Documentação oficial da Microsoft
- Criadores dos frameworks e bibliotecas utilizados

---

<div align="center">

**Desenvolvido com ❤️ e ☕ por Vitor Paiva**

[⬆ Voltar ao topo](#-sistema-de-controle-de-estoque)

</div>
