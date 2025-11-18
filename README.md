# 📝 TaskManagement — Sistema de Gestão de Tarefas Colaborativas

Resumo
------
API backend para gerenciar o ciclo de vida de tarefas em equipe. Projeto criado para demonstrar boas práticas de Engenharia de Software: arquitetura em camadas, princípios SOLID, padrões de projeto, testes unitários e preparação para CI.

Objetivo
--------
Demonstrar a aplicação prática de:
- Arquitetura em camadas clara e separação de responsabilidades.
- Princípios SOLID.
- Padrões de projeto (Factory, Strategy, Repository).
- Testes automatizados para lógica de negócio.
- Integração contínua (pipeline CI a configurar).

Tema e Funcionalidades
----------------------
- Usuário (User): cadastro e identificação de membros.
- Tarefa (Task): Título, Descrição, Responsável, Status, Datas.
- Status suportados: `Pendente`, `Em Andamento`, `Concluída`.
- API REST para CRUD de tarefas e gerenciamento de status.

Arquitetura
-----------
Segui o padrão Camadas + Web API (MVC adaptado para API), aplicando DIP para separar interfaces e implementações.

Estrutura de projetos (exemplo)
- TaskManagement.Domain — Entidades e contratos (interfaces).
- TaskManagement.Application — Serviços, regras de negócio e estratégias.
- TaskManagement.Infrastructure — Persistência (EF Core) e repositórios.
- TaskManagement.API — Controllers e configuração de IoC.
- TaskManagement.Tests.Unit — Testes unitários com xUnit + Moq.

Padrões de Projeto Aplicados
---------------------------
- Factory Method — criação de Tasks com estado inicial consistente (ex.: `Pendente`).
- Strategy — encapsula regras de transição de status (permite adicionar novas regras sem alterar TaskService).
- Repository — abstração do acesso a dados (ex.: ITaskRepository).
Esses padrões promovem SRP, OCP e DIP.

Tecnologias
-----------
- Linguagem: C# (target .NET 8)
- Web: ASP.NET Core Web API
- ORM: Entity Framework Core
- Testes: xUnit
- Mocking: Moq
- Versionamento: Git
- CI: GitHub Actions ou Azure DevOps (a ser configurado)

Testes Automatizados
--------------------
- Projeto de testes focado na camada Application.
- Uso de mocks (Moq) para isolar ITaskRepository, ITaskFactory, estratégias de status etc.
- Executar testes: dotnet test (ou via Visual Studio Test Explorer).

Como executar (rápido)
----------------------
Pré-requisitos:
- .NET 8 SDK
- Visual Studio 2022

Passos:
1. Clone:
   git clone <URL-do-repositório>
2. Abra a solução:
   Abra o arquivo __TaskManagement.sln__ no Visual Studio.
3. Restaure pacotes:
   - Visual Studio normalmente faz automaticamente; ou execute: __dotnet restore__
4. Build:
   - No Visual Studio: pressione __Ctrl+Shift+B__.
5. Executar API:
   - Defina o projeto __TaskManagement.API__ como projeto de inicialização e pressione __F5__.
   - O Swagger/OpenAPI deve abrir automaticamente para testar endpoints.

Banco de dados / EF Core
------------------------
- Configuração com DbContext (TaskManagementDbContext).
- Para criar migrações (CLI):
  - dotnet ef migrations add NomeDaMigracao --project TaskManagement.Infrastructure
  - dotnet ef database update --project TaskManagement.Infrastructure
- Alternativamente use __Package Manager Console__ no Visual Studio.

Endpoints (exemplos)
--------------------
Rotas típicas (ajustar conforme implementação):
- GET    /api/tasks
- GET    /api/tasks/{id}
- POST   /api/tasks
- PUT    /api/tasks/{id}
- DELETE /api/tasks/{id}
- POST   /api/tasks/{id}/status (atualiza status via Strategy)

Contribuição
-----------
- Abrir issues para bugs ou melhorias.
- Criar branchs temáticas (`feature/`, `fix/`, `chore/`) e PRs com descrição clara.
- Seguir convenções de código e adicionar testes para novas regras de negócio.

Observações finais
------------------
- O projeto prioriza clareza arquitetural e testabilidade.  
- Pipeline de CI e deployment podem ser adicionados (GitHub Actions/Azure DevOps) conforme necessidade.

Licença
-------
Adicionar arquivo LICENSE conforme preferir (ex.: MIT).
