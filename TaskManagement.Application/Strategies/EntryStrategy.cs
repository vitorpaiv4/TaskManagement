using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Strategies
{
    public class EntryStrategy : IMovementStrategy
    {
        public string TypeHandled => "ENTRADA";

        public void Process(Product product, Movement movement)
        {
            product.StockQuantity += movement.Quantity;
        }
    }
}
