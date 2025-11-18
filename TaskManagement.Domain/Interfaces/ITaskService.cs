using TaskManagement.Domain.Entities;
using System.Collections.Generic; // Necessário para IEnumerable<Task>

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskService
    {
        // NOVO: Método para retornar todas as tarefas, usando IEnumerable
        IEnumerable<Task> GetAllTasks();

        // Método Create (ajustado para o Padrão Factory)
        Task Create(string title, string description, int? responsibleUserId);

        // Método para atualizar o status (usa o Padrão Strategy)
        Task UpdateStatus(int taskId, string newStatus);

        // Método para buscar detalhes de uma única tarefa
        Task GetTaskDetails(int id);
    }
}