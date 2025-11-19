using System.Collections.Generic;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IMovementRepository
    {
        Movement? GetById(int id);
        IEnumerable<Movement> GetAll();
        Movement Add(Movement movement);
        void Update(Movement movement);
        void Delete(int id);
    }
}
