namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}