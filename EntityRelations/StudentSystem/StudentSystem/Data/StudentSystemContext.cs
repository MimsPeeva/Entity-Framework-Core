using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext:DbContext
    {
        private const string ConnectionString =
            "Server=DESKTOP-PI68CL1\\MSSQLSERVER01;Database=StudentSystem;Integrated Security=True;";

        public StudentSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentsCourses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Resource>()
                .Property(p => p.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);
        }
    }
}
