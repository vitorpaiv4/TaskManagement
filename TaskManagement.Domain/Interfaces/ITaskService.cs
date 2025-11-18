using TaskManagement.Domain.Entities;
using System.Collections.Generic;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskService
    {
        // NOVO: Usa TaskItem como entidade
        IEnumerable<TaskItem> GetAllTasks();

        TaskItem Create(string title, string description, int? responsibleUserId);

        TaskItem UpdateStatus(int taskId, string newStatus);

        TaskItem GetTaskDetails(int id);
    }
}