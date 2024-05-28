using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Data
{
    public class SHcx : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ExamAnswer> ExamAnswers { get; set; }
        public DbSet<ExamExecution> ExamExecutions { get; set; }
        public DbSet<ForumQuestion> ForumQuestions { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
        public DbSet<PlanExam> PlanExams { get; set; }
        public DbSet<Book> Books { get; set; }

        public SHcx(DbContextOptions<SHcx> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships here
            modelBuilder.Entity<ExamExecution>()
                .HasOne(e => e.FromSubject)
                .WithMany(s => s.ExamExecutions)
                .HasForeignKey(e => e.FromSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer

            modelBuilder.Entity<ExamExecution>()
                .HasOne(e => e.ToSubject)
                .WithMany()
                .HasForeignKey(e => e.ToSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer

            modelBuilder.Entity<PlanExam>()
                .HasOne(p => p.FromSubject)
                .WithMany()
                .HasForeignKey(p => p.FromSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer

            modelBuilder.Entity<PlanExam>()
                .HasOne(p => p.ToSubject)
                .WithMany()
                .HasForeignKey(p => p.ToSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer
        }

    }
}
