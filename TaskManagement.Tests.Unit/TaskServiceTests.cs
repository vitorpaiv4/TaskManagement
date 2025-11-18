using Xunit;
using Moq;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Entities;
using System.Collections.Generic;

namespace TaskManagement.Tests.Unit.Services
{
    public class TaskServiceTests
    {
        [Fact]
        public void Create_ShouldUseFactoryToSetStatusToPendingAndCallRepository()
        {
            var mockRepository = new Mock<ITaskRepository>();
            var mockFactory = new Mock<ITaskFactory>();

            mockFactory
                .Setup(f => f.CreateTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Returns((string title, string desc, int? userId) => new TaskItem
                {
                    Title = title,
                    Status = "Pendente"
                });

            TaskItem? savedTask = null;
            mockRepository
                .Setup(r => r.Add(It.IsAny<TaskItem>()))
                .Callback<TaskItem>(t => savedTask = t)
                .Returns<TaskItem>(t => t);

            var service = new TaskService(mockRepository.Object, mockFactory.Object, new List<ITaskStatusStrategy>());

            var result = service.Create("Comprar leite", "No supermercado", 1);

            Assert.Equal("Pendente", result.Status);

            mockFactory.Verify(f => f.CreateTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()), Times.Once);

            mockRepository.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Once);
        }
    }
}