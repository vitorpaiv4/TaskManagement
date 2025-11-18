using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskFactory
    {
        TaskItem CreateTask(string title, string description, int? responsibleUserId);
    }
}