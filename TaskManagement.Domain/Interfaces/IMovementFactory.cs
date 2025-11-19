using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IMovementFactory
    {
        Movement CreateMovement(int productId, int quantity, string type);
    }
}
