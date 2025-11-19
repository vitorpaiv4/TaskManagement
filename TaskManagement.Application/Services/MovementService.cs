using System.Collections.Generic;
using System.Linq;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class MovementService : IMovementService
    {
        private readonly IMovementRepository _movementRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMovementFactory _movementFactory;
        private readonly IEnumerable<IMovementStrategy> _movementStrategies;

        public MovementService(
            IMovementRepository movementRepository,
            IProductRepository productRepository,
            IMovementFactory movementFactory,
            IEnumerable<IMovementStrategy> movementStrategies)
        {
            _movementRepository = movementRepository;
            _productRepository = productRepository;
            _movementFactory = movementFactory;
            _movementStrategies = movementStrategies;
        }

        public Movement ProcessNewMovement(int productId, int quantity, string type)
        {
            var product = _productRepository.GetById(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Produto com ID {productId} não encontrado.");
            }

            var strategy = _movementStrategies.FirstOrDefault(s => s.TypeHandled == type);

            if (strategy == null)
            {
                throw new ArgumentException($"Tipo de movimentação '{type}' não é suportado.");
            }

            var movement = _movementFactory.CreateMovement(productId, quantity, type);

            strategy.Process(product, movement);

            _productRepository.Update(product);
            _movementRepository.Add(movement);

            return movement;
        }

        public Movement? GetMovementDetails(int id)
        {
            return _movementRepository.GetById(id);
        }
    }
}
