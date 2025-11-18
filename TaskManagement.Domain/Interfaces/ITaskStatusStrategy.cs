using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskStatusStrategy
    {
        void HandleStatusChange(TaskItem task, string newStatus);
        string StatusHandled { get; }
    }
}