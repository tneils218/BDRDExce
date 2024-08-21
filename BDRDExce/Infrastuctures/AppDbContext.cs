namespace BDRDExce.Infrastuctures;

using Applications.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Teacher>    Teachers    { get; set; }
    public DbSet<Student>    Students    { get; set; }
    public DbSet<Post>       Posts       { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Thiết lập quan hệ 1-N giữa Teacher và Student
        modelBuilder.Entity<Teacher>()
                    .HasMany(t => t.Students)
                    .WithOne(s => s.Teacher)
                    .HasForeignKey(s => s.TeacherId);

        // Thiết lập quan hệ 1-N giữa Post và Submission
        modelBuilder.Entity<Post>()
                    .HasMany(p => p.Submissions)
                    .WithOne(s => s.Post)
                    .HasForeignKey(s => s.PostId);

        // Thiết lập quan hệ 1-N giữa Student và Submission
        modelBuilder.Entity<Student>()
                    .HasMany(s => s.Submissions)
                    .WithOne(sub => sub.Student)
                    .HasForeignKey(sub => sub.StudentId);
    }
}
