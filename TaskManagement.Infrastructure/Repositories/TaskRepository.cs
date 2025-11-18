using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Entities; // Garante que você está usando a Entidade Task

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        // CORRIGIDO: Usa apenas 'Task' em vez do nome completo
        public Task Add(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public void Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        // CORRIGIDO: Usa apenas 'Task' em vez do nome completo
        public IEnumerable<Task> GetAll()
        {
            return _context.Tasks.Include(t => t.ResponsibleUser).ToList();
        }

        // CORRIGIDO: Usa apenas 'Task' em vez do nome completo
        public Task GetById(int id)
        {
            return _context.Tasks.Include(t => t.ResponsibleUser).FirstOrDefault(t => t.Id == id);
        }

        // CORRIGIDO: Usa apenas 'Task' em vez do nome completo
        public void Update(Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
    }
}