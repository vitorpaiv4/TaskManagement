// Define as operações básicas de acesso a dados (CRUD)
using System.Collections.Generic;
using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        DomainTask GetById(int id);
        IEnumerable<DomainTask> GetAll();
        DomainTask Add(DomainTask task);
        void Update(DomainTask task);
        void Delete(int id);
    }
}