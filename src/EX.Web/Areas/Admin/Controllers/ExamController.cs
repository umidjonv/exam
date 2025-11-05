using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Web.Areas.Admin.Models;
using EX.Web.Mappers;
using EX.Web.Models;
using EX.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EX.Web.Areas.Admin.Controllers
{
    public class ExamController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly HandbookService _handbook;

        private async Task LoadViewData()
        {

            #region Question Types

            var categories = await _db.Categories.ToArrayAsync();
            var questionTypes = new Dictionary<int, QuestionType>();
            var newCategories = new List<Category>();

            foreach (var category in categories)
            {
                var questions = _db.Questions.Where(x => x.CategoryId == category.Id);
                if (await questions.AnyAsync())
                {
                    newCategories.Add(category);
                }

                var baseQuestions = await questions.CountAsync(x => x.IsBase);
                var addQuestions = await questions.CountAsync(x => !x.IsBase);

                if (baseQuestions > 0 || addQuestions > 0)
                {
                    questionTypes.Add(category.Id, new QuestionType
                    {
                        BaseQuestions = baseQuestions,
                        AddQuestions = addQuestions
                    });
                }
            }
            ViewBag.QuestionTypes = questionTypes;

            #endregion

            ViewBag.Cultures = new SelectList(await _handbook.GetCultures(), "Id", "Name");
            ViewBag.CategoryList = new SelectList(newCategories, "Id", "Name");
        }

        public ExamController(IAppDbContext db, HandbookService handbook)
        {
            _db = db;
            _handbook = handbook;
        }

        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 5;
            var pageNumber = page ?? 1;
            var pager = await _db.Exams.ToPagedListAsync(pageNumber, pageSize);

            return View(pager);
        }

        [HttpPost]

        public IActionResult LanguageAccept(string lang, int[] categoryIds)
        {

            if (lang == null)
                return Json(false);
            foreach (var categoryId in categoryIds)
            {
                var questions = _db.Questions
                    .Where(x => x.CategoryId == categoryId)
                    .Include(x => x.Locales).ToList();

                var localeCount = 0;
                var questionsCount = questions.Count;
                foreach (var question in questions)
                {
                    localeCount += question.Locales.Count(x => x.CultureId == lang);
                }

                if (questionsCount != localeCount)
                    return Json(false);
            }

            return Json(true);
        }


        public async Task<IActionResult> Form(int? id)
        {
            var model = new ExamViewModel();


            if (id > 0)
            {
                var entity = await _db.Exams.Where(a => a.Id == id)
                    .Include(a => a.Options)
                    .ThenInclude(a => a.Category)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return NotFound();
                }

                model = entity.ToModel();
            }

            await LoadViewData();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, ExamViewModel model, int[] deletedOptions)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewData();


                return View(model);
            }

            if (model.Options.Count == 0)
            {
                await LoadViewData();
                ModelState.AddModelError("", $"Specify at least one question");

                return View(model);
            }

            var maxScore = model.Options.Sum(x => x.CountOfBaseQuestions + x.CountOfRegularQuestions) ?? 0;
            if (maxScore < model.PassingScore)
            {
                ModelState.AddModelError("PassingScore", $"Passing score must be less than {maxScore} ");

                await LoadViewData();

                return View(model);
            }





            var entity = model.ToEntity();

            if (id > 0)
            {

                if (id != model.Id)
                {
                    return NotFound();
                }



                foreach (var option in deletedOptions)
                {
                    var currentOption = _db.ExamOptions.FirstOrDefault(x => x.Id == option);
                    if (currentOption != null)
                    {
                        entity.Options.RemoveAll(x => x.Id == currentOption.Id);
                        _db.ExamOptions.Remove(currentOption);
                    }
                }




                _db.Exams.Update(entity);
                await _db.SaveChangesAsync();

            }
            else
            {
                entity.Code = $"{Guid.NewGuid():N}";

                _db.Exams.Add(entity);

                await _db.SaveChangesAsync();

                var currentYear = DateTime.Now.ToString("yy");
                var lastId = entity.Id;

                entity.Code = $"{lastId:D4}{currentYear}";

                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _db.Exams.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;

            _db.Exams.Update(entity);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
