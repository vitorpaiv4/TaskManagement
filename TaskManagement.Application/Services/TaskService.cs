using System.Collections.Generic;
using System.Linq; // Necessário para o FirstOrDefault no Strategy
using DomainTask = TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskFactory _taskFactory;
        // Dependência da coleção de Estratégias (para o Padrão Strategy)
        private readonly IEnumerable<ITaskStatusStrategy> _statusStrategies;

        // Construtor completo com Injeção de Dependência para todos os Contratos (DIP)
        public TaskService(
            ITaskRepository taskRepository,
            ITaskFactory taskFactory,
            IEnumerable<ITaskStatusStrategy> statusStrategies)
        {
            _taskRepository = taskRepository;
            _taskFactory = taskFactory;
            _statusStrategies = statusStrategies;
        }

        // 1. Método Create (Utiliza o Padrão Factory Method)
        public DomainTask Create(string title, string description, int? responsibleUserId) // Mudamos a assinatura para receber parâmetros, não o objeto Task completo
        {
            // Delega a criação à Fábrica, que aplica as regras iniciais (Status = Pendente)
            var newTask = _taskFactory.CreateTask(title, description, responsibleUserId);

            return _taskRepository.Add(newTask);
        }

        // 2. Método UpdateStatus (Utiliza o Padrão Strategy)
        public DomainTask UpdateStatus(int taskId, string newStatus)
        {
            var task = _taskRepository.GetById(taskId);

            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {taskId} não encontrada.");
            }

            // Encontra a Estratégia correta para o novo status
            var strategy = _statusStrategies.FirstOrDefault(s => s.StatusHandled == newStatus);

            if (strategy == null)
            {
                // Se não houver estratégia, o status é inválido ou não suportado
                throw new ArgumentException($"Transição de status '{newStatus}' não é suportada ou a estratégia não foi implementada/registrada.");
            }

            // Executa a lógica de validação/transição definida na Estratégia (OCP)
            strategy.HandleStatusChange(task, newStatus);

            _taskRepository.Update(task);
            return task;
        }

        // 3. Método GetTaskDetails (Lógica de leitura simples)
        public DomainTask GetTaskDetails(int id)
        {
            return _taskRepository.GetById(id);
        }
    }
}