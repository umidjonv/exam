using System.Linq;
using EX.Data.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EX.Web.Mappers;
using EX.Web.Models;
using Microsoft.EntityFrameworkCore;
using EX.Web.Services;
using X.PagedList;
using System;
using System.Collections.Generic;
using EX.Data.Entities;
using EX.Web.Areas.Admin.Models;
using EX.Web.Consts;
using EX.Web.Dtos;

namespace EX.Web.Areas.Admin.Controllers
{
    public class ResultController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly ExamService _service;
        private readonly IdentityService _identity;

        public ResultController(IAppDbContext db, ExamService service, IdentityService identity)
        {
            _db = db;
            _service = service;
            _identity = identity;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _db.ExamResults
                .Include(e => e.Exam)
                .Include(e => e.User)
                .Include(e => e.Units)
                .ThenInclude(x => x.Answer)
                .ThenInclude(x => x.Corrects)
                .Include(e => e.Schedule)
                .OrderByDescending(x => x.Schedule.StartDate)
                .ToPagedListAsync(page, SiteConst.PageSize);
            var users = new Dictionary<string, UserEntityDto>();
            foreach (var item in model)
            {
                var id = item.UserId;

                if (!users.ContainsKey(id))
                {
                    var user = await _identity.GetUser(id);

                    users.Add(id, user);
                }
            }

            var pager = new PagerUserViewModel<ExamResult>
            {
                Items = model,
                Users = users,
                Current = await _identity.GetUser(UserId)
            };

            return View(pager);
        }

        public async Task<IActionResult> Form(int? id)
        {

            var model = new ExamResultViewModel();

            if (!(id > 0))
                return View(model);
            var entity = await _db.ExamResults
                .Where(a => a.Id == id)
                .Include(e => e.Exam)
                .Include(e => e.User)
                .Include(e => e.Units)
                .Include(e => e.Schedule)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            model = entity.ToModel();

            var correctSelectedAnswersCount = 0;

            foreach (var answer in model.Units)
            {

                if (answer.AnswerId <= 0)
                    continue;

                var question = _db.Questions.Where(x => x.Id == answer.QuestionId)
                    .Include(x => x.Locales)
                    .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.Corrects)
                    .FirstOrDefault()
                    .ToModel();

                var questionLocale = question.Locales.FirstOrDefault(w => w.CultureId == entity.Exam.CultureId);
                var selectedAnswer = questionLocale?.Answers?.FirstOrDefault(x => x.Id == answer.AnswerId);
                var correctAnswer = questionLocale?.Answers?.FirstOrDefault(x => x.IsCorrect);

                if (selectedAnswer != null && selectedAnswer.IsCorrect)
                    correctSelectedAnswersCount += question.Weight ?? 0;

                if (selectedAnswer != null)
                    ViewData.Add("selected_" + selectedAnswer.Id, "yes");

                if (correctAnswer != null)
                    ViewData.Add("correct_" + correctAnswer.Id, "yes");

                if (selectedAnswer != null)
                {
                    ViewData.Add(answer.QuestionId.ToString(), selectedAnswer.IsCorrect ? "true" : "false");
                }
                else
                {
                    ViewData.Add(answer.QuestionId.ToString(), selectedAnswer == null ? "not_selected" : "selected");
                }
            }

            var userInExam = _db.UserInExams.Where(x => x.ScheduleId == entity.Schedule.Id && x.UserId == entity.UserId).Include(x => x.Client).FirstOrDefault();
            if (userInExam?.Client == null)
                return NotFound();

            ViewBag.Lang = SiteConst.DefaultCulture;
            ViewBag.TotalCorrectCount = correctSelectedAnswersCount + "/" + model.Exam.PassingScore;

            try
            {
                ViewBag.Files = await _service.Files(entity.UserId, userInExam.Client.UserSession);
            }
            catch
            {
                // ignored
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, bool? action, ExamResultViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (model == null)
            {
                return NotFound();
            }

            var result = _db.ExamResults.FirstOrDefault(x => x.Id == model.Id);

            if (action != null)
            {
                result.IsAccepted = action;

                if ((bool)action)
                {
                    result.Status = Data.Enums.ResultStatus.Pass;
                }
                else
                {
                    result.Status = Data.Enums.ResultStatus.Failed;
                }
            }

            result.StatusDate = DateTime.Now;
            result.StatusReason = model.StatusReason;

            _db.ExamResults.Update(result);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Download(string userId, string file)
        {
            var stream = await _service.Download(userId, file);

            return File(stream, "video/mp4");
        }

    }
}