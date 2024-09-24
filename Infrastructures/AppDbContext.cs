namespace BDRDExce.Infrastuctures;
 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
 
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext()
    {
    }
 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
 
    }
 
    public DbSet<Comment> Comments { get; init; }
    public DbSet<Course> Courses { get; init; }
    public DbSet<Submission> Submissions { get; init; }
    public DbSet<ExamMedia> ExamMedias { get; init; }
    public DbSet<SubmissionMedia> SubmissionMedias { get; init; }
    public DbSet<Media> Medias { get; init; }
    public DbSet<Exam> Exams { get; init; }
 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.DOB).HasMaxLength(10);
            entity.Property(e => e.AvatarUrl).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
        });
 
        builder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Content).HasMaxLength(255);
 
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
 
        builder.Entity<Course>(entity =>
        {
            entity.Property(e => e.Title)
                  .HasMaxLength(255);
 
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
 
            

            entity.Property(e => e.Label).HasMaxLength(100);
        });
 
        builder.Entity<Submission>(entity =>
        {
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
 
            entity.HasOne(e => e.Exam)
                  .WithMany()
                  .HasForeignKey(e => e.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
 
            entity.HasMany(e => e.Medias)
                  .WithMany()
                  .UsingEntity<SubmissionMedia>(
                      l => l.HasOne<Media>().WithMany().HasForeignKey(e => e.MediaId),
                      r => r.HasOne<Submission>().WithMany().HasForeignKey(e => e.SubmissionId));
        });
 
        builder.Entity<Media>(entity =>
        {
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.ContentName).HasMaxLength(100);
 
        });

        builder.Entity<Exam>(entity => 
        {
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.HasOne(e => e.Course)
                  .WithMany(c => c.Exams)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Medias)
                  .WithMany()
                  .UsingEntity<ExamMedia>(
                      l => l.HasOne<Media>().WithMany().HasForeignKey(e => e.MediaId),
                      r => r.HasOne<Exam>().WithMany().HasForeignKey(e => e.ExamId));
            entity.HasMany(e => e.Comments)
                  .WithOne()
                  .HasForeignKey(e => e.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
 
    }
}