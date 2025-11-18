using System.Collections.Generic;
using System.Text.Json.Serialization; // Importe este namespace

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        [JsonIgnore]
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}