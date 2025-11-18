using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Strategies
{
    public class CompletedStatusStrategy : ITaskStatusStrategy
    {
        public string StatusHandled => "Concluída";

        public void HandleStatusChange(TaskItem task, string newStatus)
        {
            if (task.Status == "Pendente" || task.Status == "Em Andamento")
            {
                task.Status = newStatus;
            }
            else
            {
                throw new InvalidOperationException($"Não é possível mudar o status de '{task.Status}' diretamente para 'Concluída'.");
            }
        }
    }
}