using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Strategies
{
    public class ExitStrategy : IMovementStrategy
    {
        public string TypeHandled => "SAÍDA";

        public void Process(Product product, Movement movement)
        {
            if (product.StockQuantity < movement.Quantity)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {product.StockQuantity}, Solicitado: {movement.Quantity}");
            }

            product.StockQuantity -= movement.Quantity;
        }
    }
}
