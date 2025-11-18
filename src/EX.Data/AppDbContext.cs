using EX.Data.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EX.Data.Entities;

namespace EX.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        private readonly IHttpContextAccessor _httpContext;

        private void ApplyAuditValues()
        {
            var entries = ChangeTracker.Entries()
               .Where(e => e.Entity is AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            // user
            var userName = _httpContext?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = Environment.UserName ?? "(local)";
            }

            // ip
            var userIp = _httpContext?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            if (string.IsNullOrWhiteSpace(userIp))
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                userIp = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "::1";
            }

            foreach (var entityEntry in entries)
            {
                ((AuditEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;
                ((AuditEntity)entityEntry.Entity).ModifiedIp = userIp;
                ((AuditEntity)entityEntry.Entity).ModifiedBy = userName;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((AuditEntity)entityEntry.Entity).CreatedIp = userIp;
                    ((AuditEntity)entityEntry.Entity).CreatedBy = userName;
                }
            }

        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContext)
           : base(options)
        {
            _httpContext = httpContext;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserInExams> UserInExams { get; set; }

        public DbSet<UserInSession> UserInSessions { get; set; }

        public DbSet<UserInClient> UserInClients { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionLocale> QuestionLocales { get; set; }

        public DbSet<QuestionInAnswer> QuestionInAnswers { get; set; }

        public DbSet<QuestionInCorrect> QuestionInCorrects { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<ExamOption> ExamOptions { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<AnswerUnit> AnswerUnits { get; set; }

        public DbSet<ExamResult> ExamResults { get; set; }

        public DbSet<Document> Documents { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditValues();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditValues();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Document>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<AnswerUnit>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.Question).WithMany(a => a.Units).HasForeignKey(a => a.QuestionId);
                schema.HasOne(a => a.Result).WithMany(a => a.Units).HasForeignKey(a => a.ResultId);
                schema.HasOne(a => a.Answer).WithMany(a => a.Units).HasForeignKey(a => a.AnswerId);
            });

            modelBuilder.Entity<Category>(schema =>
            {
                schema.HasKey(a => a.Id);
                 
                schema.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<Exam>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasIndex(a => a.Code).IsUnique();

                schema.HasQueryFilter(p => !p.IsDeleted);
            });

            modelBuilder.Entity<ExamOption>(schema =>
            {
                schema.HasKey(a => a.Id);
                schema.HasQueryFilter(p => !p.IsDeleted);

                schema.HasOne(a => a.Exam).WithMany(a => a.Options).HasForeignKey(a => a.ExamId);
                schema.HasOne(a => a.Category).WithMany(a => a.Options).HasForeignKey(a => a.CategoryId);
            });

            modelBuilder.Entity<ExamResult>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.Exam).WithMany(a => a.Results).HasForeignKey(a => a.ExamId);
                schema.HasOne(a => a.Schedule).WithMany(a => a.Results).HasForeignKey(a => a.ScheduleId);
                schema.HasOne(a => a.User).WithMany(a => a.Results).HasForeignKey(a => a.UserId);
            });

            modelBuilder.Entity<Question>(schema =>
            {
                schema.HasKey(a => a.Id);
                schema.HasQueryFilter(p => !p.IsDeleted);

                schema.HasOne(a => a.Category).WithMany(a => a.Questions).HasForeignKey(a => a.CategoryId);
            });

            modelBuilder.Entity<QuestionInAnswer>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.QuestionLocale).WithMany(a => a.Answers).HasForeignKey(a => a.QuestionLocaleId);
            });

            modelBuilder.Entity<QuestionInCorrect>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.Answer).WithMany(a => a.Corrects).HasForeignKey(a => a.AnswerId);
                schema.HasOne(a => a.Question).WithMany(a => a.Corrects).HasForeignKey(a => a.QuestionId);
            });

            modelBuilder.Entity<QuestionLocale>(schema =>
            {
                schema.HasKey(a => a.Id);
            });
            
            modelBuilder.Entity<Schedule>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasQueryFilter(p => !p.IsDeleted);

                schema.HasOne(a => a.Exam).WithMany(a => a.Schedules).HasForeignKey(a => a.ExamId);
            });

            modelBuilder.Entity<User>(schema =>
            {
                schema.HasKey(a => a.Id); 
            });
             
            modelBuilder.Entity<UserInClient>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasIndex(a => a.UserSession).IsUnique();

                schema.HasOne(a => a.User).WithMany(a => a.Clients).HasForeignKey(a => a.UserId);
            });

            modelBuilder.Entity<UserInExams>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.User).WithMany(a => a.Exams).HasForeignKey(a => a.UserId);
                schema.HasOne(a => a.Schedule).WithMany(a => a.Users).HasForeignKey(a => a.ScheduleId);
                schema.HasOne(a => a.Exam).WithMany(a => a.Users).HasForeignKey(a => a.ExamId);
                schema.HasOne(a => a.Client).WithMany(a => a.Exams).HasForeignKey(a => a.ClientId);
            });

            modelBuilder.Entity<UserInSession>(schema =>
            {
                schema.HasKey(a => a.Id);

                schema.HasOne(a => a.User).WithMany(a => a.Sessions).HasForeignKey(a => a.UserId);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
