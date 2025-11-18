using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Domain.Interfaces
{
    // O Contrato para as diferentes estratégias de manipulação de status
    public interface ITaskStatusStrategy
    {
        // Verifica se a transição de status é válida e aplica a lógica
        void HandleStatusChange(DomainTask task, string newStatus);

        // Propriedade para identificar a estratégia
        string StatusHandled { get; }
    }
}