// Define as operações de negócio que os Controllers irão chamar
using DomainTask = TaskManagement.Domain.Entities.Task;
namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskService
    {
        // Exemplo: O Service deve encapsular regras, como criar com status padrão
        DomainTask Create(string title, string description, int? responsibleUserId);
        DomainTask UpdateStatus(int taskId, string newStatus);
        DomainTask GetTaskDetails(int id);
        // ... outros métodos de negócio
    }
}
