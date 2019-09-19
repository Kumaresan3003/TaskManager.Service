namespace TaskManager.Service.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<TaskDetailModel> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DOTNET; Database = TaskManagerDB; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskDetailModel>().HasKey("Id");
            builder.Entity<TaskDetailModel>().ToTable("Task");
            builder.Entity<TaskDetailModel>().Property(p => p.Name).HasColumnName("Task").HasMaxLength(100).IsRequired();
            builder.Entity<TaskDetailModel>().Property(p => p.ParentTaskId).HasColumnName("ParentId");
            builder.Entity<TaskDetailModel>().Property(p => p.StartDate).HasColumnName("Start_Date").IsRequired();
            builder.Entity<TaskDetailModel>().Property(p => p.EndDate).HasColumnName("End_Date").IsRequired();
            builder.Entity<TaskDetailModel>().Property(p => p.Priority).IsRequired();                       
            builder.Entity<TaskDetailModel>().Property(p => p.EndTask).HasColumnName("EndTask").IsRequired();
        }
    }
}