using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EX.Api.Services
{
    public class ExamService
    {
        private readonly IAppDbContext _db;

        public ExamService(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<UserInExams> Last(string userId)
        {
            var exam = await _db.UserInExams
                .Include(a => a.User)
                .Where(x => x.UserId == userId && x.IsPassed == false && x.IsAdmit)
                .Include(a => a.Schedule)
                .Where(x => x.Schedule.Mode == ExamMode.Online)
                .Include(a => a.Exam)
                .OrderByDescending(a => a.RequestTime)
                .FirstOrDefaultAsync();

            return exam;
        }

        public async Task<UserInExams> Active(string userId)
        {
            var exam = await _db.UserInExams
                .Include(a => a.User)
                .Where(x => x.UserId == userId && x.IsPassed == false && x.IsAdmit)
                .Include(a => a.Schedule)
                .Where(x => x.Schedule.Mode == ExamMode.Online && (x.Schedule.Status == ScheduleStatus.Active || x.Schedule.Status == ScheduleStatus.New|| x.Schedule.Status == ScheduleStatus.Pending))
                .Include(a => a.Exam)
                .OrderByDescending(a => a.RequestTime)
                .FirstOrDefaultAsync();

            return exam;
        }
         
    }
}