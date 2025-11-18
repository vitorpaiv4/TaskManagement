using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using System;

namespace TaskManagement.Application.Factories
{
    public class TaskFactory : ITaskFactory
    {
        public TaskItem CreateTask(string title, string description, int? responsibleUserId)
        {
            return new TaskItem
            {
                Title = title,
                Description = description,
                ResponsibleUserId = responsibleUserId,
                Status = "Pendente",
                CreationDate = DateTime.UtcNow
            };
        }
    }
}