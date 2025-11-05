using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Data.Enums;
using EX.Web.Areas.Client.Models;
using EX.Web.Consts;
using EX.Web.Mappers;
using EX.Web.Models;
using EX.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace EX.Web.Areas.Client.Controllers
{
    public class ExamController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly HandbookService _handbook;
        private ExamProgressReport _progressReport;
        private readonly ExamService _service;
        private readonly CacheService _cache;
        private readonly IHtmlLocalizer<ExamController> _localizer;
        private readonly ExamService _examService;

        #region Mange caching stores

        private void GetStore()
        {
            var key = $"_exam_report_{UserId}_";

            _progressReport = _cache.Get<ExamProgressReport>(key);
        }

        private void SetStore(ExamProgressReport store)
        {
            var key = $"_exam_report_{UserId}_";

            _cache.Set(key, store);
        }

        private void ClearStore()
        {
            var key = $"_exam_report_{UserId}_";

            _cache.Remove<ExamProgressReport>(key);

            _progressReport = null;
        }

        private async Task ExamOnError(string errorMessage)
        {
            if (_progressReport != null)
            {


                var result = await _db.ExamResults.FirstOrDefaultAsync(x => x.Id == _progressReport.ResultId);

                if (result == null)
                    return;

                result.ErrorMessage += (DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + errorMessage + "\r\n");
                await _db.SaveChangesAsync();
            }
        }

        #endregion

        private async Task<User> CurrentUser()
        {
            return await _db.Users.FindAsync(UserId);
        }

        private Task<UserInClient> ActiveClient(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out _))
                return _db.Users.Where(a => a.Id == UserId)
                    .Include(a => a.Sessions)
                    .SelectMany(a => a.Clients)
                    .OrderByDescending(a => a.LoginTime)
                    .FirstOrDefaultAsync();

            return _db.Users.Where(a => a.Id == UserId)
                .Include(a => a.Clients)
                .SelectMany(a => a.Clients)
                .Where(w => w.UserSession == id)
                .OrderByDescending(a => a.LoginTime)
                .FirstOrDefaultAsync();
        }

        private async Task<bool> CheckAvailableClient(UserInExams userInExam, string session = "")
        {
            var client = await ActiveClient(session);

            if (client == null)
            {
                return false;
            }

            userInExam.ClientId = client.Id;

            _db.UserInExams.Update(userInExam);
            await _db.SaveChangesAsync();

            return client.IsConnected ?? false;
        }

        private async Task<bool> CheckOnlineClient(UserInExams userInExams)
        {
            if (userInExams.Schedule.Mode == ExamMode.Online)
            {
                var userInClient = await ActiveClient("");

                if (userInClient == null || userInClient.IsConnected == false)
                {
                    return false;
                }

                userInExams.ClientId = userInClient.Id;

                _db.UserInExams.Update(userInExams);
                await _db.SaveChangesAsync();

                return userInClient.IsConnected ?? false;
            }

            return true;
        }

        private static int GetNextPage(int pageCount, int curPage)
        {
            if (pageCount > curPage)
                curPage++;
            else if (pageCount < curPage)
                curPage--;

            return curPage;
        }

        private int[] GenerateQuestions(int examId, string lang = SiteConst.DefaultCulture, bool shuffle = true)
        {
            Trace.WriteLine($"Question lang:{lang}");

            var exam = _db.Exams
                .Where(x => x.Id == examId)
                .Include(x => x.Options)
                .FirstOrDefault();
            var generatedQuestions = new List<int>();

            foreach (var option in exam.Options)
            {

                var questions = _db.Questions.Where(x => x.CategoryId == option.CategoryId && x.IsBase)
                    .Select(x => new
                    {
                        x.Id,
                        Locales = x.Locales.Where(c => c.CultureId == exam.CultureId),
                    });

                var questionsBase = questions.Where(x => x.Locales.Any()).Select(x => x.Id).Take(option.CountOfBaseQuestions).ToList();
                generatedQuestions.AddRange(questionsBase);

                questions = _db.Questions.Where(x => x.CategoryId == option.CategoryId && !x.IsBase)
                    .Select(x => new
                    {
                        x.Id,
                        Locales = x.Locales.Where(c => c.CultureId == exam.CultureId),
                    });


                var questionsRegular = questions.Where(x => x.Locales.Any()).Select(x => x.Id).Take(option.CountOfRegularQuestions).ToList();
                generatedQuestions.AddRange(questionsRegular);

            }

            return shuffle
                ? generatedQuestions.OrderBy(x => Guid.NewGuid()).ToArray()
                : generatedQuestions.ToArray();
        }

        private async Task GenerateAnswers()
        {
            if (_progressReport.ResultId.HasValue)
            {
                for (var i = 0; i < _progressReport.QuestionIds.Length; i++)
                {
                    var questionId = _progressReport.QuestionIds[i];

                    _db.AnswerUnits.Add(new AnswerUnit
                    {
                        QuestionId = questionId,
                        AnswerId = null,
                        ResultId = (int)_progressReport.ResultId
                    });

                }
                await _db.SaveChangesAsync();
            }

        }

        private async Task StartResultReport(ExamProgressReport examReport)
        {
            var user = await CurrentUser();
            var exam = await _db.UserInExams.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ScheduleId == examReport.ScheduleId);

            var examResult = new ExamResult()
            {
                ExamId = exam.ExamId,
                ScheduleId = examReport.ScheduleId,
                UserId = user.Id,
                StartTime = DateTime.Now,
                QuestionIds = JsonConvert.SerializeObject(examReport.QuestionIds)
            };

            await _db.ExamResults.AddAsync(examResult);
            await _db.SaveChangesAsync();

            examReport.ResultId = examResult.Id;

        }

        private void AddAnswerToAnswerUnit(int page, int questionId)
        {

            if (_progressReport.ResultId != null)
            {
                var resultId = _progressReport.ResultId.Value;
                var answerUnit = _db.AnswerUnits
                    .FirstOrDefault(x => x.QuestionId == questionId && x.ResultId == resultId);

                if (answerUnit == null)
                {

                    if (_progressReport.Answered.ContainsKey(page))
                    {
                        var answer = _db.AnswerUnits.FirstOrDefault(x => x.ResultId == _progressReport.ResultId && x.QuestionId == questionId);

                        if (answer != null)
                        {
                            answer.AnswerId = _progressReport.Answered[page].Id;
                            _db.AnswerUnits.Update(answer);
                        }
                    }
                }
                else
                {
                    answerUnit.QuestionId = questionId;
                    answerUnit.AnswerId = _progressReport.Answered[page].Id;
                    answerUnit.ResultId = resultId;
                }

                _db.SaveChanges();
            }
        }

        private void SetCorrectCount(int page, int answerId, QuestionViewModel question)
        {
            if (_progressReport != null)
            {
                var questionLocale = question.Locales.FirstOrDefault(w => w.CultureId == _progressReport.CultureId);
                var answer = questionLocale?.Answers?.FirstOrDefault(x => x.Id == answerId);

                if (answer != null)
                {

                    if (_progressReport.Answered.ContainsKey(page))
                    {

                        answer.Weight = question.Weight ?? 0;
                        _progressReport.Answered[page] = answer;
                    }
                    else if (!_progressReport.Answered.ContainsKey(page))
                    {
                        answer.Weight = question.Weight ?? 0;
                        _progressReport.Answered.Add(page, answer);
                    }
                }

            }

        }

        private void ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

        }

        public ExamController(IAppDbContext db, HandbookService handbook, ExamService service, CacheService cache
            , IHtmlLocalizer<ExamController> localizer, ExamService examService)
        {
            _db = db;
            _handbook = handbook;
            _service = service;
            _cache = cache;
            _localizer = localizer;
            _examService = examService;
        }

        public async Task<IActionResult> Index(int page = 1, int? region = null)
        {
            GetStore();

            var query = _db.Schedules.Where(x => x.Status != ScheduleStatus.Finished)
                .Include(a => a.Users)
                .ThenInclude(a => a.User)
                .OrderByDescending(a => a.StartDate)
                .AsQueryable();

            if (region > 0)
                query = query.Where(w => w.RegionId == region);

            if (_progressReport != null)
            {
                var schedule = query.FirstOrDefault(x => x.Id == _progressReport.ScheduleId);
                if (schedule == null)
                    ClearStore();
            }

            var model = new ExamListingViewModel
            {
                Regions = new SelectList(await _handbook.GetRegions(), "Id", "Name", region),
                Items = await query.Include(x => x.Exam).ToPagedListAsync(page, SiteConst.PageSize),
                UserId = UserId,
                ScheduleId = _progressReport?.ScheduleId ?? 0,
                ExamProgressStatus = _progressReport?.Status ?? ResultStatus.NotStarted,
                CurrentPosition = _progressReport?.CurrentPosition ?? 0
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DisEnroll(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await CurrentUser();

            if (user == null)
                return NotFound();

            var schedule = await _db.Schedules
                .Where(x => x.Id == id)
                .Include(x => x.Users)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync();

            if (schedule == null)
                return NotFound();

            var userInExams = await _db.UserInExams.FirstOrDefaultAsync(x => x.IsAdmit && x.ScheduleId == id && x.UserId == user.Id);

            if (userInExams != null)
            {
                _db.UserInExams.Remove(userInExams);

                await _db.SaveChangesAsync();
            }

            TempData["error"] = _localizer.GetString("UnsubscribedFromExam").Value;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public new async Task<IActionResult> Request(int? id)
        {

            if (id == null)
                return NotFound();

            var user = await CurrentUser();

            if (user == null)
                return NotFound();

            var schedule = await _db.Schedules
                .Where(x => x.Id == id)
                .Include(x => x.Users)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync();

            if (schedule == null)
                return NotFound();

            var isAdmit = true;
            var countOfParticipants = schedule.Users.Count(x => x.IsAdmit);
            var message = "";

            if (schedule.LimitOfParticipants <= countOfParticipants)
            {
                message = _localizer.GetString("NoVacantSeats").Value;
                isAdmit = false;
            }

            if (_db.UserInExams.Any())
            {
                var userExam = await _db.UserInExams
                    .Where(x => x.ExamId == schedule.ExamId && x.UserId == user.Id && x.IsPassed == true)
                    .Include(x => x.Schedule)
                    .Include(x => x.Exam)
                    .FirstOrDefaultAsync();

                if (userExam != null)
                {
                    var period = schedule.StartDate.Subtract(userExam.Schedule.StartDate.AddDays(userExam.Exam.PeriodInDays)).Days;

                    if (period < 0)
                    {
                        isAdmit = false;

                        message = string.Format(_localizer["NextExamTime"].Value, Math.Abs(period));
                    }
                }
            }


            if (isAdmit)
            {
                _db.UserInExams.Add(new UserInExams
                {
                    ScheduleId = schedule.Id,
                    IsPassed = false,
                    RequestTime = DateTime.Now,
                    UserId = user.Id,
                    IsAdmit = true,
                    ExamId = schedule.ExamId
                });

                await _db.SaveChangesAsync();

                if (schedule.Mode == ExamMode.Online)
                {
                    try
                    {
                        await _examService.Assign(UserId);
                    }
                    catch (Exception e)
                    {
                        message = e.Message;
                    }
                }
            }

            TempData["error"] = message;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Welcome(int id, int scheduleId, string lang = SiteConst.DefaultCulture)
        {           
            try
            {
                GetStore();

                if (_progressReport != null)
                {
                    if (_progressReport.ScheduleId != scheduleId)
                        ClearStore();
                }

                if (_progressReport == null)
                {
                    if (scheduleId == 0)
                        return NotFound();

                    var user = await CurrentUser();
                    if (user == null)
                        return NotFound();

                    var userInExam = _db.UserInExams
                        .Where(x => x.UserId == user.Id && x.ScheduleId == scheduleId)
                        .Include(x => x.Schedule)
                        .Include(x => x.Exam)
                        .ThenInclude(x => x.Options)
                        .FirstOrDefault();

                    if (userInExam == null)
                        return NotFound();

                    //ChangeLanguage(userInExam.Exam.CultureId);

                    var examViewModel = _db.Exams.FirstOrDefault(x => x.Id == id)?
                        .ToModel();

                    if (examViewModel == null)
                    {

                        return RedirectToAction(nameof(Index), new
                        {
                            message = _localizer.GetString("ExamNotFound").Value
                        });
                    }

                    lang = userInExam.Exam.CultureId;
                    ViewBag.scheduleId = scheduleId;

                    var questions = GenerateQuestions(id, userInExam.Exam.CultureId, false);
                    if (questions.Length == 0)
                        return NotFound();


                    ViewBag.QuestionsCount = questions.Length;

                    return View(examViewModel);

                }

                ViewBag.lang = lang;
                ViewBag.Langs = await _handbook.GetCultures();

                return RedirectToAction(nameof(Question), new
                {
                    page = _progressReport.CurrentPosition
                });
            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                TempData["error"] = exception.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Start(int id, int scheduleId)
        {
            try
            {

                GetStore();

                if (_progressReport != null)
                {
                    if (_progressReport.ScheduleId != scheduleId)
                        ClearStore();
                }

                if (scheduleId == 0)
                    return NotFound();

                var user = await CurrentUser();
                if (user == null)
                    return NotFound();

                var userInExam = _db.UserInExams
                    .Where(x => x.UserId == user.Id && x.ScheduleId == scheduleId)
                    .Include(x => x.Schedule)
                    .Include(x => x.Exam)
                    .ThenInclude(x => x.Options)
                    .FirstOrDefault();


                if (userInExam == null)
                    return RedirectToAction(nameof(Index), new { message = _localizer.GetString("ExamNotFound").Value });

                if (_progressReport == null)
                {


                    if (!await CheckOnlineClient(userInExam))
                    {
                        return RedirectToAction(nameof(Index), new { message = _localizer.GetString("ExamNotFound").Value });
                    }


                    var questions = GenerateQuestions(id, userInExam.Exam.CultureId);
                    if (questions.Length == 0)
                        return NotFound();

                    var examReport = new ExamProgressReport
                    {
                        CurrentPosition = 1,
                        QuestionIds = questions,
                        ScheduleId = scheduleId,
                        CultureId = userInExam.Exam.CultureId
                    };


                    await StartResultReport(examReport);


                    _progressReport = examReport;
                    _progressReport.Status = ResultStatus.Active;
                    _progressReport.TestScore = userInExam.Exam.PassingScore;

                    await GenerateAnswers();

                    SetStore(examReport);

                }

                return RedirectToAction(nameof(Question), new
                {
                    page = _progressReport.CurrentPosition
                });
            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                return RedirectToAction(nameof(Index), new { message = exception.Message, messageType = 1 });
            }

        }

        public async Task<IActionResult> Start(string session)
        {
            try
            {
                GetStore();

                if (session == null)
                    return NotFound();

                var currentUser = await CurrentUser();
                var userInExam = await _db.UserInExams
                    .Where(x => x.IsAdmit && x.IsPassed == false && x.UserId == currentUser.Id)
                    .Include(x => x.Schedule)
                    .Where(x => x.Schedule.Mode == ExamMode.Online && x.Schedule.Status == ScheduleStatus.Active)
                    .Include(x => x.Exam)
                    .ThenInclude(x => x.Options)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                if (userInExam == null)
                {
                    TempData["error"] = _localizer["ExamNotFound"].Value;

                    return RedirectToAction(nameof(Index));
                }


                

                if (!await CheckAvailableClient(userInExam, session))
                {
                    TempData["error"] = "Нет подключенного настольного приложения.";

                    return RedirectToAction(nameof(Index));
                }

                if (userInExam.Schedule.Mode == ExamMode.Online)
                {
                    await _service.Listen(UserId);
                }

                if (_progressReport != null)
                {
                    if (_progressReport.ScheduleId != userInExam.ScheduleId)
                        ClearStore();
                }

                if (_progressReport == null)
                {
                    var scheduleId = userInExam.ScheduleId;

                    if (userInExam.Exam == null)
                        return NotFound();

                    

                    var examViewModel = userInExam.Exam.ToModel();




                    ViewBag.scheduleId = scheduleId;
                    var questions = GenerateQuestions(userInExam.ExamId, userInExam.Exam.CultureId);

                    if (questions.Length == 0)
                        return NotFound();

                    ViewBag.QuestionsCount = questions.Length;

                    return View("Welcome", examViewModel);
                }

                ViewBag.lang = _progressReport.CultureId;
                ViewBag.Langs = await _handbook.GetCultures();

                return RedirectToAction(nameof(Question), new
                {
                    page = _progressReport.CurrentPosition
                });

            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                TempData["error"] = exception.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(int pageNumber, int answerId, int nextPage = 0, bool isFinish = false)
        {
            try
            {


                GetStore();

                var page = pageNumber;

                if (_progressReport != null && _progressReport.QuestionIds.Count() > page - 1)
                {
                    var questionId = _progressReport.QuestionIds[page - 1];

                    var question = _db.Questions.Where(x => x.Id == questionId)
                        .Include(x => x.Locales)
                        .ThenInclude(x => x.Answers)
                        .ThenInclude(x => x.Corrects)
                        .FirstOrDefault()
                        .ToModel();
                    if (answerId > 0)
                    {
                        SetCorrectCount(page, answerId, question);

                        SetStore(_progressReport);

                        AddAnswerToAnswerUnit(page, questionId);
                    }

                    if (isFinish)
                        return RedirectToAction(nameof(Finish));

                    var prevPage = page;
                    if (nextPage != 0)
                    {
                        page = nextPage;
                    }
                    else
                    {
                        page = GetNextPage(_progressReport.QuestionIds.Length, page);


                        if (prevPage == page)
                            return RedirectToAction(nameof(Question), new
                            {
                                page,
                                isNextPage = true
                            });
                    }

                    return RedirectToAction(nameof(Question), new { page });
                }

                return NotFound();
            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                TempData["error"] = exception.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Question(int page, bool isNextPage = false)
        {
            try
            {


                GetStore();

                if (_progressReport != null && _progressReport.QuestionIds.Count() > page - 1)
                {

                    page = page == 0 ? _progressReport.CurrentPosition : page;

                    var questionId = _progressReport.QuestionIds[page - 1];

                    _progressReport.CurrentPosition = page;

                    var question = _db.Questions.Where(x => x.Id == questionId)
                        .Include(x => x.Locales)
                        .ThenInclude(x => x.Answers)
                        .FirstOrDefault();

                    if (question != null)
                    {
                        foreach (var locale in question.Locales)
                        {
                            locale.Answers = locale.Answers.OrderBy(x => x.Order).ToList();
                        }
                    }

                    var questionLocale = question.ToModel()
                        .Locales
                        .Where(w => w.CultureId == _progressReport.CultureId)?
                        .FirstOrDefault();


                    var user = await CurrentUser();

                    var userInExam = await _db.UserInExams
                        .Where(x => x.UserId == user.Id && x.ScheduleId == _progressReport.ScheduleId)
                        .Include(x => x.Schedule)
                        .Include(x => x.Exam)
                        .ThenInclude(x => x.Options)
                        .FirstOrDefaultAsync();

                    if (userInExam != null && userInExam.Schedule.Status == ScheduleStatus.Finished)
                    {
                        ClearStore();
                        TempData["error"] = _localizer["ExamNotFound"].Value;

                        return RedirectToAction(nameof(Index));
                    }


                    if (!await CheckOnlineClient(userInExam))
                    {
                        TempData["error"] = _localizer.GetString("DesktopNotConnected").Value;
                    }


                    if (userInExam?.Schedule != null)
                    {
                        var durationInMinutes = userInExam.Exam?
                            .DurationInMinutes;

                        if (durationInMinutes != null)
                        {
                            ViewBag.finishDate = userInExam.Schedule.StartDate.AddMinutes((double)durationInMinutes)
                                .ToUniversalTime();
                        }
                    }

                    ViewBag.pages = _progressReport.QuestionIds.ToPagedList(page, 1);
                    ViewBag.answered = _progressReport.Answered.ContainsKey(page)
                        ? _progressReport.Answered[page].Id
                        : -1;
                    ViewBag.IsNext = isNextPage;

                    ViewBag.answeredPages = _progressReport.Answered.Keys.ToArray();
                    ViewBag.questionNumber = page;
                    ViewBag.isFinish = _progressReport.QuestionIds.Length <= _progressReport.Answered.Count;


                    return View(questionLocale);
                }

                return NotFound();
            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                TempData["error"] = exception.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Next(int page)
        {
            GetStore();

            if (_progressReport != null)
            {
                page = GetNextPage(_progressReport.QuestionIds.Count(), page);

                return RedirectToAction(nameof(Question), new { page });
            }

            return NotFound();
        }

        public async Task<IActionResult> Finish()
        {
            try
            {

                GetStore();

                if (_progressReport != null)
                {

                    _progressReport.CorrectSelectedAnswersCount =
                        _progressReport.Answered.Where(x => x.Value.IsCorrect).Sum(x => x.Value.Weight);


                    _progressReport.IncorrectSelectedAnswersCount =
                        _progressReport.Answered.Where(x => !x.Value.IsCorrect).Sum(x => x.Value.Weight);


                    _progressReport.Status = _progressReport.TestScore <= _progressReport.CorrectSelectedAnswersCount
                        ? ResultStatus.Pass
                        : ResultStatus.Failed;
                    _progressReport.ResultMessage = _localizer.GetString($"{_progressReport.Status}ResultMessage").Value;

                    var user = await CurrentUser();
                    var userExam = await _db.UserInExams
                        .Where(x => x.ScheduleId == _progressReport.ScheduleId && x.UserId == user.Id)
                        .Include(x => x.Schedule)
                        .Include(x => x.Exam)
                        .FirstOrDefaultAsync();

                    if (userExam?.Schedule != null)
                    {
                        var result = await _db.ExamResults.FirstOrDefaultAsync(x => x.Id == _progressReport.ResultId);


                        if (result != null)
                        {

                            result.FinishTime = DateTime.Now;
                            var schedule = userExam.Schedule;

                            var exceptIds = _progressReport.QuestionIds.Except(_progressReport.Answered.Keys);

                            var notAnswered = _db.Questions.Where(x => exceptIds.Contains(x.Id)).Sum(x => x.Weight);

                            CalculateResults(result, schedule.Mode, notAnswered, _progressReport);

                            _db.ExamResults.Update(result);
                            await _db.SaveChangesAsync();
                        }
                    }

                    if (userExam != null)
                    {
                        userExam.IsPassed = true;

                        await _db.SaveChangesAsync();
                         
                        if (userExam.Schedule?.Mode == ExamMode.Online)
                        {
                            try
                            {
                                await _examService.Finish(UserId);
                            }
                            catch (Exception e)
                            {
                                TempData["error"] = e.Message;
                            }
                        }

                    }

                    var report = _progressReport;

                    ViewBag.lang = _progressReport.CultureId;

                    ClearStore();

                    return View(report);
                }

                return NotFound();
            }
            catch (Exception exception)
            {
                await ExamOnError(exception.Message);

                TempData["error"] = exception.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        public void CalculateResults(ExamResult result, ExamMode examMode, int? notAnswered, ExamProgressReport progressReport)
        {
            switch (examMode)
            {
                case ExamMode.Standard when string.IsNullOrWhiteSpace(result.ErrorMessage):
                    {
                        if (progressReport.CorrectSelectedAnswersCount >= progressReport.TestScore)
                        {
                            result.Status = ResultStatus.Pass;
                            result.IsAccepted = true;
                        }
                        else
                        {
                            result.Status = ResultStatus.Failed;

                            result.IsAccepted = false;
                        }

                        break;
                    }
                case ExamMode.Standard when progressReport.CorrectSelectedAnswersCount >= progressReport.TestScore:
                    {
                        result.Status = ResultStatus.Pass;
                        result.IsAccepted = true;
                    }
                    break;
                case ExamMode.Standard when notAnswered.HasValue:
                    {
                        var mustAnsweredCount = 0;
                        if (notAnswered > 0)
                            mustAnsweredCount = (int)(progressReport.CorrectSelectedAnswersCount + notAnswered);

                        result.Status = (mustAnsweredCount) < progressReport.TestScore
                            ? ResultStatus.Failed
                            : ResultStatus.Error;
                        break;
                    }
                case ExamMode.Standard:
                    {
                        result.Status = ResultStatus.Failed;
                        result.IsAccepted = false;
                    }
                    break;
                case ExamMode.Online:
                    {
                        result.Status = progressReport.CorrectSelectedAnswersCount >= progressReport.TestScore
                            ? ResultStatus.Pass
                            : ResultStatus.Failed;

                        if (!(progressReport.CorrectSelectedAnswersCount >= progressReport.TestScore))
                            result.IsAccepted = false;
                        break;
                    }
            }
        }

    }
}