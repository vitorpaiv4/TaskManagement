using System.Collections.Generic;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        TaskItem? GetById(int id);
        IEnumerable<TaskItem> GetAll();
        TaskItem Add(TaskItem task);
        void Update(TaskItem task);
        void Delete(int id);
    }
}