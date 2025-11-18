using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Web.Areas.Admin.Models;
using EX.Web.Mappers;
using EX.Web.Models;
using EX.Web.Services;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ClosedXML.Excel;
using EX.Web.Consts;


namespace EX.Web.Areas.Admin.Controllers
{
    public class QuestionController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly HandbookService _handbook;

        private async Task LoadViewData(QuestionViewModel model)
        {
            ViewBag.CategoryList = new SelectList(await _db.Categories.ToArrayAsync(), "Id", "Name", model.CategoryId);
        }

        public QuestionController(IAppDbContext db, HandbookService handbook)
        {
            _db = db;
            _handbook = handbook;
        }

        [Route("[area]/[controller]/{lang?}/{page?}")]
        public async Task<IActionResult> Index(string lang = SiteConst.DefaultCulture, int page = 1)
        {
            var questions = await _db.Questions
                .Include(a => a.Locales)
                .Where(w => w.Locales.Any(a => a.CultureId == lang))
                .Include(a => a.Category).ToListAsync();



            IEnumerable<QuestionViewModel> collection = new List<QuestionViewModel>();

            if (questions.Any())
            {
                collection = questions.ToCollection();
            }

            var pager = collection.ToPagedList(page, SiteConst.PageSize);

            var model = new PagerLocaleViewModel<QuestionViewModel>
            {
                Items = pager,
                Lang = lang,
                Cultures = await _handbook.GetCultures()
            };

            var vModel = new QuestionViewModel();

            await LoadViewData(vModel);

            return View(model);
        }


        public async Task<IActionResult> Form(int? id) 
        {
            var model = new QuestionViewModel();
            var cultures = await _handbook.GetCultures();

            if (id > 0)
            {
                var entity = await _db.Questions.Where(a => a.Id == id)
                    .Include(a => a.Locales)
                    .ThenInclude(a => a.Answers)
                    .Include(a => a.Corrects)
                    .Include(a => a.Category)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var locale in entity.Locales)
                {

                    locale.Answers = locale.Answers.OrderBy(x => x.Order).ToList();

                }

                model = entity.ToModel();

                foreach (var locale in model.Locales)
                {
                    locale.CultureName = cultures.OrderByDescending(a => a.Code).FirstOrDefault(a => a.Id == locale.CultureId).Name;
                }

                model.Locales = cultures.OrderByDescending(x => x.Code).Select((s) =>
                      {
                          var locale = model.Locales.FirstOrDefault(x => x.CultureId == s.Id);
                          if (locale != null)
                              return locale;

                          return new QuestionLocaleViewModel
                          {
                              CultureId = s.Id,
                              CultureName = s.Name
                          };


                      }
                ).ToList();

                var isHaveCategory = _db.ExamOptions.Any(x => x.CategoryId == entity.CategoryId);
                ViewBag.isHaveCategory = isHaveCategory;

            }
            else
            {
                model.Locales = cultures.Select(s => new QuestionLocaleViewModel
                {
                    CultureId = s.Id,
                    CultureName = s.Name
                }).ToList();
            }

            await LoadViewData(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewData(model);

                return View(model);
            }

            var hasError = false;

            model.Locales.ForEach((a) =>
            {
                if (!string.IsNullOrWhiteSpace(a.Title))
                {
                    var message = "";
                    if (!(a.Answers.Count >= 2))
                    {
                        message = $"Minimum two answer you need add in a {a.CultureName} !";
                        ModelState.AddModelError("", message);
                        TempData["error"] = message;

                        hasError = true;
                        return;
                    }

                    if (!a.Answers.Any(a => a.IsCorrect))
                    {
                        message = $"Minimum one answer you need choose in a {a.CultureName} !";
                        ModelState.AddModelError("", message);
                        TempData["error"] = message;

                        hasError = true;
                    }
                }
            });

            if (hasError)
            {
                await LoadViewData(model);

                return View(model);
            }

            var locales = model.Locales.Where(x => x.Title == null).ToArray();
            foreach (var item in locales) model.Locales.Remove(item);
            var entity = model.ToEntity();

            var index = 1;
            foreach (var locale in entity.Locales)
            {
                foreach (var answer in locale.Answers)
                {
                    answer.Order = index.ToString()[0];
                    index++;
                }


            }

            if (id > 0)
            {

                if (id != model.Id)
                {
                    return NotFound();
                }

                _db.Questions.Update(entity);

                // clear prev
                foreach (var locale in entity.Locales)
                {
                    var answers = await _db.QuestionInCorrects.Where(w => w.QuestionId == id && w.CultureId == locale.CultureId).ToArrayAsync();

                    _db.QuestionInCorrects.RemoveRange(answers);
                }

            }
            else
            {
                var emptyLocales = model.Locales.Where(x => x.Title != null).ToArray();
                if (emptyLocales.Length == 0)
                {
                    TempData["error"] = "Question Title must not be empty!";
                    await LoadViewData(model);

                    var cultures = await _handbook.GetCultures();
                    foreach (var locale in model.Locales)
                    {
                        locale.CultureName = cultures.OrderByDescending(a => a.Code).FirstOrDefault(a => a.Id == locale.CultureId)?.Name;
                    }

                    return View(model);
                }
                else
                {
                    _db.Questions.Add(entity);
                }

            }

            await _db.SaveChangesAsync();

            // re-arrange corrects
            foreach (var locale in entity.Locales)
            {
                var answers = model.Locales.FirstOrDefault(a => a.CultureId == locale.CultureId)?.Answers;

                if (answers == null)
                    continue;

                foreach (var answer in answers.Where(w => w.IsCorrect))
                {
                    var correct = locale.Answers.FirstOrDefault(a => a.Title == answer.Title);

                    if (correct == null)
                        continue;

                    _db.QuestionInCorrects.Add(new QuestionInCorrect
                    {
                        CultureId = locale.CultureId,
                        AnswerId = correct.Id,
                        QuestionId = entity.Id
                    });
                }
            }
            await _db.SaveChangesAsync();

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

            var entity = await _db.Questions.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;

            _db.Questions.Update(entity);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadExcel(IFormFile uploadedFile, int categoryId)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileStream = uploadedFile.OpenReadStream();
                using var workBook = new XLWorkbook(fileStream, XLEventTracking.Disabled);

                IXLWorksheet workSheetUz;
                IXLWorksheet workSheetRu;

                try
                {
                    workSheetUz = workBook.Worksheet("uz");
                    workSheetRu = workBook.Worksheet("ru");
                }
                catch (System.Exception)
                {

                    workSheetUz = null;
                    workSheetRu = null;
                }


                var firstRow = true;
                var questions = new List<Question>();
                var corrects = new List<(string, int, char)>();

                //uz

                if (workSheetUz != null)
                {
                    foreach (var row in workSheetUz.RowsUsed())
                    {
                        if (firstRow)
                        {
                            firstRow = false;
                        }
                        else
                        {
                            var question = new Question
                            {
                                CategoryId = categoryId,
                                Weight = row.Cell(5).GetValue<int>(),
                                IsBase = row.Cell(4).GetBoolean()
                            };
                            questions.Add(question);

                        }
                    }
                    _db.Questions.AddRange(questions);
                    await _db.SaveChangesAsync();

                    var i = 0;
                    firstRow = true;
                    foreach (var row in workSheetUz.RowsUsed())
                    {
                        if (firstRow)
                        {
                            firstRow = false;
                        }
                        else
                        {

                            var question = questions[i];

                            var locales = new List<QuestionLocale>();
                            var locale = new QuestionLocale
                            {
                                Title = row.Cell(2).GetString(),
                                Description = row.Cell(3).GetString(),
                                CultureId = "uz"
                            };
                            var answers = new List<QuestionInAnswer>();
                            var answerOrder = 1;
                            for (var index = 7; index <= row.LastCellUsed().Address.ColumnNumber; index++)
                            {
                                answers.Add(new QuestionInAnswer
                                {
                                    Title = row.Cell(index).GetString(),
                                    Order = answerOrder.ToString()[0]
                                });
                                answerOrder++;
                            };

                            corrects.Add(("uz", question.Id, row.Cell(6).GetString()[0]));
                            locale.Answers.AddRange(answers);
                            locales.Add(locale);
                            question.Locales.AddRange(locales);
                            i++;
                        }
                    }

                    //ru
                    if (workSheetRu != null)
                    {
                        firstRow = true;
                        i = 0;
                        foreach (var row in workSheetRu.RowsUsed())
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                            }
                            else
                            {
                                var question = questions[i];
                                var locales = new List<QuestionLocale>();
                                var locale = new QuestionLocale
                                {
                                    Title = row.Cell(2).GetString(),
                                    Description = row.Cell(3).GetString(),
                                    CultureId = "ru"
                                };
                                var answers = new List<QuestionInAnswer>();
                                var answerOrder = 1;
                                for (var index = 7; index <= row.LastCellUsed().Address.ColumnNumber; index++)
                                {
                                    answers.Add(new QuestionInAnswer
                                    {
                                        Title = row.Cell(index).GetString(),
                                        Order = answerOrder.ToString()[0]
                                    });
                                    answerOrder++;
                                };

                                corrects.Add(("ru", question.Id, row.Cell(6).GetString()[0]));
                                locale.Answers.AddRange(answers);
                                locales.Add(locale);
                                question.Locales.AddRange(locales);
                                i++;
                            }
                        }
                    }
                    if (questions.Count > 0)
                    {
                        _db.Questions.UpdateRange(questions);
                        await _db.SaveChangesAsync();

                        //corrects
                        foreach (var question in questions)
                        {
                            foreach (var correct in corrects)
                            {
                                var answer = question.Locales.FirstOrDefault(a => a.CultureId == correct.Item1)?.Answers.FirstOrDefault(a => a.Order == correct.Item3);
                                if (answer != null && question.Id == correct.Item2)
                                {
                                    question.Corrects.Add(new QuestionInCorrect
                                    {
                                        AnswerId = answer.Id,
                                        CultureId = correct.Item1
                                    });
                                }
                            }
                        }

                        _db.Questions.UpdateRange(questions);

                        await _db.SaveChangesAsync();

                        TempData["success"] = "Imported successfully";
                    }
                    else
                    {
                        TempData["error"] = "Empty Excel File!";
                    }
                }
                else
                {
                    TempData["error"] = "Please select correct excel file!";
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}