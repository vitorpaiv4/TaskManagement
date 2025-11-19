using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories
{
    public class MovementRepository : IMovementRepository
    {
        private readonly TaskManagementDbContext _context;

        public MovementRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public Movement Add(Movement movement)
        {
            _context.Movements.Add(movement);
            _context.SaveChanges();
            return movement;
        }

        public void Delete(int id)
        {
            var movement = _context.Movements.Find(id);
            if (movement != null)
            {
                _context.Movements.Remove(movement);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Movement> GetAll()
        {
            return _context.Movements.AsNoTracking().Include(m => m.Product).ToList();
        }

        public Movement? GetById(int id)
        {
            return _context.Movements.AsNoTracking().Include(m => m.Product).FirstOrDefault(m => m.Id == id);
        }

        public void Update(Movement movement)
        {
            _context.Movements.Update(movement);
            _context.SaveChanges();
        }
    }
}
