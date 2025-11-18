using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TaskManagement.Application.Services
{
    // O TaskService implementa o Contrato (ITaskService) e contém a Lógica de Negócios.
    public class TaskService : ITaskService
    {
        // Dependências injetadas via construtor (DIP)
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskFactory _taskFactory;
        private readonly IEnumerable<ITaskStatusStrategy> _statusStrategies;

        // Construtor com Injeção de Dependência para todas as abstrações
        public TaskService(
            ITaskRepository taskRepository,
            ITaskFactory taskFactory,
            IEnumerable<ITaskStatusStrategy> statusStrategies)
        {
            _taskRepository = taskRepository;
            _taskFactory = taskFactory;
            _statusStrategies = statusStrategies;
        }

        // 1. Implementação GetAllTasks
        public IEnumerable<TaskItem> GetAllTasks()
        {
            // Delega a chamada ao repositório
            return _taskRepository.GetAll();
        }

        // 2. Implementação Create (Utiliza o Padrão Factory Method)
        public TaskItem Create(string title, string description, int? responsibleUserId)
        {
            // Delega a criação à Fábrica, que aplica as regras iniciais (Ex: Status = Pendente)
            var newTask = _taskFactory.CreateTask(title, description, responsibleUserId);

            return _taskRepository.Add(newTask);
        }

        // 3. Implementação UpdateStatus (Utiliza o Padrão Strategy)
        public TaskItem UpdateStatus(int taskId, string newStatus)
        {
            var task = _taskRepository.GetById(taskId);

            if (task == null)
            {
                // KeyNotFoundException é usada no .NET para indicar que uma chave não foi encontrada
                throw new KeyNotFoundException($"Tarefa com ID {taskId} não encontrada.");
            }

            // Encontra a Estratégia correta para o novo status
            var strategy = _statusStrategies.FirstOrDefault(s => s.StatusHandled == newStatus);

            if (strategy == null)
            {
                // ArgumentException para indicar que um parâmetro (status) é inválido
                throw new ArgumentException($"Transição de status '{newStatus}' não é suportada ou a estratégia não foi implementada/registrada.");
            }

            // Executa a lógica de validação/transição definida na Estratégia (cumprindo OCP)
            strategy.HandleStatusChange(task, newStatus);

            _taskRepository.Update(task);
            return task;
        }

        // 4. Implementação GetTaskDetails
        public TaskItem GetTaskDetails(int id)
        {
            return _taskRepository.GetById(id);
        }
    }
}