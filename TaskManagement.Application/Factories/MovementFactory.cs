using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Factories
{
    public class MovementFactory : IMovementFactory
    {
        public Movement CreateMovement(int productId, int quantity, string type)
        {
            return new Movement
            {
                ProductId = productId,
                Quantity = quantity,
                Type = type,
                Date = DateTime.UtcNow
            };
        }
    }
}
