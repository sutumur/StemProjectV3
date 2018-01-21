using StemProjectV3.Models;
using Microsoft.EntityFrameworkCore;

namespace StemProjectV3.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Mentor>().ToTable("Mentor");
            modelBuilder.Entity<ProjectAssignment>().ToTable("ProjectAssignment");

            modelBuilder.Entity<ProjectAssignment>().HasKey(p => new { p.ProjectID, p.MentorID });
        }
    }
}
