using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using EX.Data.Entities;

namespace EX.Data.Core
{
    public interface IAppDbContext
    {

        DbSet<User> Users { get; set; }

        DbSet<UserInExams> UserInExams { get; set; }

        DbSet<UserInSession> UserInSessions { get; set; }
        
        DbSet<UserInClient> UserInClients{ get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<Question> Questions { get; set; }

        DbSet<QuestionLocale> QuestionLocales { get; set; }

        DbSet<QuestionInAnswer> QuestionInAnswers { get; set; }

        DbSet<QuestionInCorrect> QuestionInCorrects { get; set; }

        DbSet<Exam> Exams { get; set; }

        DbSet<ExamOption> ExamOptions { get; set; }

        DbSet<Schedule> Schedules { get; set; }

        DbSet<AnswerUnit> AnswerUnits { get; set; }

        DbSet<ExamResult> ExamResults { get; set; }

        DbSet<Document>  Documents { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}