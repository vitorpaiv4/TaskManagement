using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities; // Referencia as entidades criadas no projeto Domain

namespace TaskManagement.Infrastructure.Data
{
    public class TaskManagementDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }

        // Define as tabelas (DbSets) que serão mapeadas para as entidades
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        // Método opcional para configurar modelos (Fluent API)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exemplo de configuração: garantir que o Email do Usuário é único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configurar a relação entre Task e User
            modelBuilder.Entity<Task>()
                .HasOne(t => t.ResponsibleUser) // Uma Task tem um ResponsibleUser
                .WithMany(u => u.AssignedTasks) // Um User tem muitas AssignedTasks
                .HasForeignKey(t => t.ResponsibleUserId) // Chave estrangeira
                .IsRequired(false); // O ResponsibleUser pode ser opcional (nullable)

            base.OnModelCreating(modelBuilder);
        }
    }
}