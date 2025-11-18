namespace TaskManagement.Domain.Entities
{
    // A classe pura que representa um Usuário no sistema
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Armazenar senha com hash é fundamental

        // Lista de tarefas atribuídas a este usuário (opcional, mas útil)
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    }
}