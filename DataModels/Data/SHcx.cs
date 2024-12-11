using DataModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;


namespace DataModels.Data
{
    public class SHcx : IdentityDbContext<User, Role, int>
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ExamAnswer> ExamAnswers { get; set; }
        public DbSet<ExamExecution> ExamExecutions { get; set; }
        public DbSet<ForumQuestion> ForumQuestions { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
        public DbSet<ExamPlan> PlanExams { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<SubjectText> SubjectTexts { get; set; }
        public DbSet<BaseQuestion> BaseQuestions { get; set; }
        public DbSet<UserQuestion> UserQuestions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<DeletedRecord> DeletedRecords { get; set; }
        public SHcx(DbContextOptions<SHcx> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<ExamPlan>()
                .HasOne(p => p.FromSubject)
                .WithMany()
                .HasForeignKey(p => p.FromSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer

            modelBuilder.Entity<ExamPlan>()
                .HasOne(p => p.ToSubject)
                .WithMany()
                .HasForeignKey(p => p.ToSubjectId)
                .OnDelete(DeleteBehavior.Restrict); // or whatever delete behavior you prefer

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.UserSettings);

            // Configure one-to-one relationship
            modelBuilder.Entity<Subject>()
                .HasOne(m => m.SubjectText)
                .WithOne(e => e.Subject)
                .HasForeignKey<SubjectText>(e => e.SubjectId);

            // Optional: Configure primary key for ExtraEntity if it's not automatically inferred
            modelBuilder.Entity<SubjectText>()
                .HasKey(e => e.SubjectId);

            // Configure inheritance mapping for BaseQuestion using the custom discriminator property
            modelBuilder.Entity<BaseQuestion>()
                .HasDiscriminator<string>("DiscriminatorRF")
                .HasValue<Question>("Question")
                .HasValue<UserQuestion>("UserQuestion");

            // Configure relationships for derived classes
            modelBuilder.Entity<UserQuestion>()
                .HasOne(uq => uq.User)
                .WithMany()
                .HasForeignKey(uq => uq.UserId);

            modelBuilder.Entity<UserQuestion>()
                .HasOne(uq => uq.Question)
                .WithMany(q => q.DerivedUserQuestions)
                .HasForeignKey(uq => uq.OriginalQuestionId);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditable.DateCreatedAudit))
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<IAuditable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreatedAudit = DateTime.UtcNow;
                    entry.Entity.LastUpdatedAudit = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdatedAudit = DateTime.UtcNow;
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
