using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ToDoItemModel> ToDoItem { get; set; }
        public DbSet<PriorityModel> Priority { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriorityModel>()
                .HasKey(p => p.ToDoItemId);

            modelBuilder.Entity<PriorityModel>()
                .HasOne(p => p.ToDoItem)
                .WithOne(t => t.PriorityModel)
                .HasForeignKey<PriorityModel>(p => p.ToDoItemId);

            modelBuilder.Entity<UserModel>().ToTable("User");
            modelBuilder.Entity<ToDoItemModel>().ToTable("ToDoItem");
            modelBuilder.Entity<PriorityModel>().ToTable("Priority");
        }
    }
}
