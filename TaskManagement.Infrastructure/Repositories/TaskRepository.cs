using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public TaskItem Add(TaskItem task)
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

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.Tasks.AsNoTracking().Include(t => t.ResponsibleUser).ToList();
        }

        public TaskItem? GetById(int id)
        {
            return _context.Tasks.AsNoTracking().Include(t => t.ResponsibleUser).FirstOrDefault(t => t.Id == id);
        }

        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
    }
}