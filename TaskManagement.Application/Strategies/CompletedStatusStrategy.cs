using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Strategies
{
    public class CompletedStatusStrategy : ITaskStatusStrategy
    {
        public string StatusHandled => "Concluída";

        public void HandleStatusChange(Task task, string newStatus)
        {
            // Lógica de Negócios específica para a transição para "Concluída"
            if (task.Status == "Pendente" || task.Status == "Em Andamento")
            {
                task.Status = newStatus;
                // Exemplo de lógica: registrar a data de conclusão
                // task.CompletionDate = DateTime.UtcNow; 
            }
            else
            {
                throw new InvalidOperationException($"Não é possível mudar o status de '{task.Status}' diretamente para 'Concluída'.");
            }
        }
    }
}