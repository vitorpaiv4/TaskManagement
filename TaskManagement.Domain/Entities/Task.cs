namespace TaskManagement.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int? ResponsibleUserId { get; set; }
        public User? ResponsibleUser { get; set; }

        public string Status { get; set; } = "Pendente";

        public DateTime CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}