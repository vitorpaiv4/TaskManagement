using Xunit; // Framework de teste
using Moq; // Framework de Mocking
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Entities;
using System.Collections.Generic;

namespace TaskManagement.Tests.Unit.Services
{
    // A classe de teste segue o padrão [NomeDaClasseSendoTestada]Tests
    public class TaskServiceTests
    {
        // Convenção: Usa a anotação [Fact] para definir um método de teste
        [Fact]
        public void Create_ShouldUseFactoryToSetStatusToPendingAndCallRepository()
        {
            // ARRANGE (Preparação)
            // 1. Criar Mocks para as dependências (DIP em ação: não precisamos do Repository/Factory real)
            var mockRepository = new Mock<ITaskRepository>();
            var mockFactory = new Mock<ITaskFactory>();

            // 2. Configurar o Mock da Factory: Quando for chamado, simular a criação da Tarefa
            // Usamos Setup para simular o comportamento
            mockFactory
                .Setup(f => f.CreateTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Returns((string title, string desc, int? userId) => new Task
                {
                    Title = title,
                    Status = "Pendente" // Simula a regra de negócio do Factory
                });

            // 3. Configurar o Mock do Repository: Simular o salvamento no banco
            Task savedTask = null;
            mockRepository
                .Setup(r => r.Add(It.IsAny<Task>()))
                .Callback<Task>(t => savedTask = t) // Captura o objeto Task que está sendo salvo
                .Returns<Task>(t => t);

            // 4. Cria a instância do Service, injetando os Mocks (a classe que queremos testar)
            var service = new TaskService(mockRepository.Object, mockFactory.Object, new List<ITaskStatusStrategy>());
            // Passamos uma lista vazia de Strategies, pois não estamos testando a Strategy neste momento

            // ACT (Ação)
            var result = service.Create("Comprar leite", "No supermercado", 1);

            // ASSERT (Verificação)
            // 1. O status está correto? (Testando a regra de negócio)
            Assert.Equal("Pendente", result.Status);

            // 2. O Factory foi chamado exatamente uma vez? (Testando o uso do Padrão)
            mockFactory.Verify(f => f.CreateTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()), Times.Once);

            // 3. O Repository foi chamado para salvar a tarefa?
            mockRepository.Verify(r => r.Add(It.IsAny<Task>()), Times.Once);
        }
    }
}