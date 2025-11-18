using System.Collections.Generic;
using System.Linq;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskFactory _taskFactory;
        private readonly IEnumerable<ITaskStatusStrategy> _statusStrategies;

        public TaskService(
            ITaskRepository taskRepository,
            ITaskFactory taskFactory,
            IEnumerable<ITaskStatusStrategy> statusStrategies)
        {
            _taskRepository = taskRepository;
            _taskFactory = taskFactory;
            _statusStrategies = statusStrategies;
        }

        public TaskItem Create(string title, string description, int? responsibleUserId)
        {
            var newTask = _taskFactory.CreateTask(title, description, responsibleUserId);
            return _taskRepository.Add(newTask);
        }

        public TaskItem UpdateStatus(int taskId, string newStatus)
        {
            var task = _taskRepository.GetById(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {taskId} não encontrada.");
            }

            var strategy = _statusStrategies.FirstOrDefault(s => s.StatusHandled == newStatus);

            if (strategy == null)
            {
                throw new ArgumentException($"Transição de status '{newStatus}' não é suportada ou a estratégia não foi implementada/registrada.");
            }

            strategy.HandleStatusChange(task, newStatus);

            _taskRepository.Update(task);
            return task;
        }

        public TaskItem? GetTaskDetails(int id)
        {
            return _taskRepository.GetById(id);
        }
    }
}