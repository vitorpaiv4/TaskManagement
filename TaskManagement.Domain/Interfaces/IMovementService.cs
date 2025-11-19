using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IMovementService
    {
        Movement ProcessNewMovement(int productId, int quantity, string type);
        Movement? GetMovementDetails(int id);
    }
}
