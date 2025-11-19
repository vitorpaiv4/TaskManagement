using Xunit;
using Moq;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Tests.Unit.Services
{
    public class MovementServiceTests
    {
        [Fact]
        public void ProcessNewMovement_ShouldUseFactoryAndStrategy()
        {
            var mockMovementRepo = new Mock<IMovementRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockFactory = new Mock<IMovementFactory>();

            var product = new Product
            {
                Id = 1,
                Sku = "SKU001",
                Name = "Product Test",
                StockQuantity = 10
            };

            mockProductRepo
                .Setup(r => r.GetById(1))
                .Returns(product);

            mockFactory
                .Setup(f => f.CreateMovement(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns((int productId, int qty, string type) => new Movement
                {
                    ProductId = productId,
                    Quantity = qty,
                    Type = type,
                    Date = DateTime.UtcNow
                });

            Movement? savedMovement = null;
            mockMovementRepo
                .Setup(r => r.Add(It.IsAny<Movement>()))
                .Callback<Movement>(m => savedMovement = m)
                .Returns<Movement>(m => m);

            var entryStrategy = new Mock<IMovementStrategy>();
            entryStrategy.Setup(s => s.TypeHandled).Returns("ENTRADA");
            entryStrategy.Setup(s => s.Process(It.IsAny<Product>(), It.IsAny<Movement>()));

            var strategies = new List<IMovementStrategy> { entryStrategy.Object };

            var service = new MovementService(
                mockMovementRepo.Object,
                mockProductRepo.Object,
                mockFactory.Object,
                strategies
            );

            var result = service.ProcessNewMovement(1, 5, "ENTRADA");

            Assert.Equal("ENTRADA", result.Type);
            Assert.Equal(5, result.Quantity);

            mockFactory.Verify(f => f.CreateMovement(1, 5, "ENTRADA"), Times.Once);
            mockMovementRepo.Verify(r => r.Add(It.IsAny<Movement>()), Times.Once);
            entryStrategy.Verify(s => s.Process(product, It.IsAny<Movement>()), Times.Once);
        }

        [Fact]
        public void ExitStrategy_ShouldFailWhenStockIsZero()
        {
            var product = new Product
            {
                Id = 1,
                StockQuantity = 0
            };

            var movement = new Movement
            {
                ProductId = 1,
                Quantity = 5,
                Type = "SAÍDA"
            };

            var exitStrategy = new TaskManagement.Application.Strategies.ExitStrategy();

            Assert.Throws<InvalidOperationException>(() => exitStrategy.Process(product, movement));
        }

        [Fact]
        public void MovementFactory_ShouldSetTypeAndDate()
        {
            var factory = new TaskManagement.Application.Factories.MovementFactory();

            var movement = factory.CreateMovement(1, 10, "ENTRADA");

            Assert.Equal("ENTRADA", movement.Type);
            Assert.Equal(1, movement.ProductId);
            Assert.Equal(10, movement.Quantity);
            Assert.True((DateTime.UtcNow - movement.Date).TotalSeconds < 5);
        }
    }
}