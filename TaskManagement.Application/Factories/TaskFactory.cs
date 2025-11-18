using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Factories
{
    // A Fábrica implementa o Contrato e garante a criação consistente
    public class TaskFactory : ITaskFactory
    {
        public Task CreateTask(string title, string description, int? responsibleUserId)
        {
            // Lógica de Criação Centralizada:
            return new Task
            {
                Title = title,
                Description = description,
                ResponsibleUserId = responsibleUserId,
                Status = "Pendente", // Regra: Seta o status inicial padrão
                CreationDate = DateTime.UtcNow // Regra: Seta a data de criação
            };
        }
    }
}