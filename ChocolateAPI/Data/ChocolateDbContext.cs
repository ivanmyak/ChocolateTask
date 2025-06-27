using ChocolateAPI.Entites;
using Microsoft.EntityFrameworkCore;

namespace ChocolateAPI.Data
{
    public class ChocolateDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        public ChocolateDbContext(DbContextOptions<ChocolateDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация сущностей: индексы, ограничения, enum → string и т.д.
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<string>();
        }
    }
}
