using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskFactory
    {
        // Define o método para criar uma Task com os dados essenciais
        DomainTask CreateTask(string title, string description, int? responsibleUserId);
    }
}