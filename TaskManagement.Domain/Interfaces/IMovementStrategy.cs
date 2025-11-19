using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IMovementStrategy
    {
        void Process(Product product, Movement movement);
        string TypeHandled { get; }
    }
}
