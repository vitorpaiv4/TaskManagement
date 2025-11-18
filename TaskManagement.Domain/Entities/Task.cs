namespace TaskManagement.Domain.Entities
{
    // A classe pura que representa uma Tarefa no sistema
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Chave estrangeira para o Usuário responsável
        public int? ResponsibleUserId { get; set; }

        // Propriedade de navegação (opcional, mas útil para EF Core)
        public User ResponsibleUser { get; set; }

        // O status da tarefa (Pendete, Em Andamento, Concluída)
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? DueDate { get; set; } // Opcional
    }
}