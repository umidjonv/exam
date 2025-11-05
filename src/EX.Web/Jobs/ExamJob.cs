using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Data.Enums;
using EX.Web.Extensions;
using EX.Web.Models;
using EX.Web.Services;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EX.Web.Jobs
{
    public class ExamJob : BackgroundService
    {
        private readonly IServiceProvider _service;
        private static readonly TimeSpan SleepTime = TimeSpan.FromMinutes(1);
        private readonly IHtmlLocalizer<ExamJob> _localizer;
        private readonly ILogger<ExamJob> _logger;

        public ExamJob(IServiceProvider service, IHtmlLocalizer<ExamJob> localizer, ILogger<ExamJob> logger)
        {
            _service = service;
            _localizer = localizer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _service.CreateScope())
                    {

                        var nowDateTime = DateTime.Now;
                        var identityService = scope.ServiceProvider.GetRequiredService<IdentityService>();
                        var smsService = scope.ServiceProvider.GetRequiredService<SmsService>();
                        var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                        var cacheService = scope.ServiceProvider.GetRequiredService<CacheService>();

                        #region Sending SMS

                        var smsSchedules = await db.Schedules.Where(a => a.Status == ScheduleStatus.New)
                            .Include(a => a.Exam)
                            .Include(a => a.Exam.Options)
                            .Include(a => a.Users)
                            .ThenInclude(a => a.User)
                            .ToArrayAsync(stoppingToken);

                        if (smsSchedules.Any())
                        {
                            foreach (var schedule in smsSchedules.Where(a => a.StartDate.Subtract(nowDateTime) <= TimeSpan.FromMinutes(30)))
                            {
                                //to notify sms
                                foreach (var exam in schedule.Users.Where(x => x.IsAdmit && x.User != null))
                                {
                                    var userEntity = await identityService.GetUser(exam.UserId);
                                    var phoneNumber = userEntity.GetAttributeValue("phone_number") ?? "977111770";

                                    if (string.IsNullOrWhiteSpace(phoneNumber))
                                    {
                                        continue;
                                    }
                                    
                                    var beginDate = schedule.StartDate.ToString("dd/MM hh:mm");

                                    try
                                    {
                                        await smsService.Send(phoneNumber, $"Здравствуйте  {userEntity.LastName} {userEntity.FirstName}, ваш экзамен начнется в {beginDate}.");
                                    }
                                    catch (Exception exception)
                                    {
                                        _logger.Log(LogLevel.Error, $"Sms exception:{exception.Message}");
                                    }

                                    
                                }

                                schedule.Status = ScheduleStatus.Pending;

                                db.Schedules.Update(schedule);
                            }

                            await db.SaveChangesAsync(stoppingToken);
                        }

                        #endregion

                        #region Active schedulers

                        var schedulesPending = await db.Schedules.Where(x => x.Status == ScheduleStatus.Pending)
                            .Include(a => a.Exam)
                            .Include(a => a.Exam.Options)
                            .Include(a => a.Users)
                            .ThenInclude(a => a.User)
                            .ToArrayAsync(stoppingToken);

                        if (schedulesPending.Any())
                        {
                            foreach (var schedule in schedulesPending.Where(a => a.StartDate.Subtract(nowDateTime) <= TimeSpan.FromMinutes(1)))
                            {
                                schedule.Status = ScheduleStatus.Active;

                                db.Schedules.Update(schedule);
                            }

                            await db.SaveChangesAsync(stoppingToken);
                        }

                        #endregion

                        #region Finish schedulers

                        var activeSchedules = await db.Schedules
                            .Include(a => a.Exam)
                            .Include(a => a.Exam.Options)
                            .Where(x => x.Status == ScheduleStatus.Active)
                            .Include(a => a.Users)
                            .Include(x => x.Results)
                            .ThenInclude(a => a.User)
                            .ToArrayAsync(stoppingToken);

                        if (activeSchedules.Any())
                        {
                            var query = activeSchedules.Where(a => a.StartDate.AddMinutes(a.Exam.DurationInMinutes).Subtract(nowDateTime) <= TimeSpan.FromMinutes(-1));

                            foreach (var schedule in query)
                            {
                                // sync cache 
                                foreach (var userInExam in schedule.Users.Where(x => x.IsAdmit && x.User != null))
                                {
                                    var userKey = $"_exam_report_{userInExam.UserId}_";
                                    var report = cacheService.Get<ExamProgressReport>(userKey);
                                    if (report == null)
                                    {
                                        continue;
                                    }

                                    #region Calculate Not finished Results

                                    var result = await db.ExamResults.FirstOrDefaultAsync(x => x.Id == report.ResultId && x.FinishTime == null, cancellationToken: stoppingToken);
                                    if (result == null)
                                    {
                                        continue;
                                    }

                                    var exceptIds = report.QuestionIds.Except(report.Answered.Keys);
                                    var notAnswered = db.Questions.Where(x => exceptIds.Contains(x.Id)).Sum(x => x.Weight);

                                    report.CorrectSelectedAnswersCount = report.Answered.Where(x => x.Value.IsCorrect).Sum(x => x.Value.Weight);
                                    report.IncorrectSelectedAnswersCount = report.Answered.Where(x => !x.Value.IsCorrect).Sum(x => x.Value.Weight);

                                    if (userInExam.Client != null && string.IsNullOrWhiteSpace(userInExam.Client.ErrorMessage))
                                    {
                                        result.ErrorMessage = userInExam.Client.ErrorMessage;
                                    }

                                    CalculateResults(result, userInExam.Schedule.Mode, notAnswered, report);

                                    if (result.Status == ResultStatus.Error)
                                    {
                                        userInExam.IsPassed = false;
                                        result.ErrorMessage = string.IsNullOrWhiteSpace(result.ErrorMessage)
                                            ? "Not Connected"
                                            : result.ErrorMessage;

                                    }

                                    result.StatusReason = _localizer.GetString("NotFinished").Value;

                                    db.ExamResults.Update(result);
                                    db.UserInExams.Update(userInExam);

                                    #endregion

                                    cacheService.Remove<ExamProgressReport>(userKey);
                                }

                                schedule.Status = ScheduleStatus.Finished;

                                db.Schedules.Update(schedule);
                            }

                            await db.SaveChangesAsync(stoppingToken);

                            //remove cache
                            foreach (var user in query.SelectMany(a => a.Users))
                            {
                                var userKey = $"_exam_report_{user.Id}_";

                                cacheService.Remove<ExamProgressReport>(userKey);
                            }

                        }

                        #endregion

                    }

                    await Task.Delay(SleepTime, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.Log(LogLevel.Error, exception.Message);
                }
            }
        }

        public void CalculateResults(ExamResult result, ExamMode mode, int? notAnswered, ExamProgressReport report)
        {

            if (mode == ExamMode.Standard)
            {
                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    if (report.CorrectSelectedAnswersCount >= report.TestScore)
                    {
                        result.Status = report.CorrectSelectedAnswersCount >= report.TestScore
                            ? ResultStatus.Pass
                            : ResultStatus.Failed;
                        result.IsAccepted = report.CorrectSelectedAnswersCount >= report.TestScore;
                    }
                    else
                    {
                        var mustAnsweredCount = 0;
                        if (notAnswered > 0)
                        {
                            mustAnsweredCount = (int)(report.CorrectSelectedAnswersCount + notAnswered);
                        }

                        result.Status = (mustAnsweredCount) < report.TestScore
                            ? ResultStatus.Failed
                            : ResultStatus.Error;
                        result.IsAccepted = false;
                    }
                }
                else
                {
                    if (report.CorrectSelectedAnswersCount >= report.TestScore)
                    {
                        result.Status = ResultStatus.Pass;
                        result.IsAccepted = true;
                    }
                    else if (notAnswered.HasValue)
                    {

                        var mustAnsweredCount = 0;
                        
                        if (notAnswered > 0)
                        {
                            mustAnsweredCount = (int)(report.CorrectSelectedAnswersCount + notAnswered);
                        }

                        result.Status = mustAnsweredCount < report.TestScore
                                ? ResultStatus.Failed
                                : ResultStatus.Error;
                        result.IsAccepted = false;

                    }
                    else
                    {
                        result.Status = ResultStatus.Failed;
                        result.IsAccepted = false;
                    }

                }

            }
            else if (mode == ExamMode.Online)
            {

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    if (report.CorrectSelectedAnswersCount >= report.TestScore)
                    {
                        result.Status = ResultStatus.Pass;

                    }
                    else if (notAnswered.HasValue)
                    {
                        var mustAnsweredCount = 0;

                        if (notAnswered > 0)
                        {
                            mustAnsweredCount = (int)(report.CorrectSelectedAnswersCount + notAnswered);
                        }

                        result.Status = (mustAnsweredCount) < report.TestScore
                            ? ResultStatus.Failed
                            : ResultStatus.Error;
                        result.IsAccepted = false;
                    }
                    else
                    {
                        result.Status = ResultStatus.Failed;
                        result.IsAccepted = false;
                    }
                }
                else
                {
                    result.Status = report.CorrectSelectedAnswersCount >= report.TestScore
                        ? ResultStatus.Pass
                        : ResultStatus.Error;

                    if (!(report.CorrectSelectedAnswersCount >= report.TestScore))
                    {
                        result.IsAccepted = false;
                    }
                }

            } 
        }
    }
}