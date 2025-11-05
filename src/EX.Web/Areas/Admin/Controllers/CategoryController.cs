using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EX.Data.Core;
using EX.Web.Mappers;
using EX.Web.Models;
using Microsoft.AspNetCore.Mvc.Localization;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.Collections.Generic;
using EX.Data.Entities;
using EX.Web.Consts;
using EX.Web.Filters;
using Microsoft.EntityFrameworkCore;

namespace EX.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly IHtmlLocalizer<CategoryController> _localizer;

        private void SetUncategorizedQuestions(int categoryId, int parentId)
        {
            var questions = _db.Questions.Where(x => x.CategoryId == categoryId);

            foreach (var question in questions)
            {
                question.CategoryId = parentId;
            }

            _db.Questions.UpdateRange(questions);
        }
        public CategoryController(IAppDbContext db, IHtmlLocalizer<CategoryController> localizer)
        {
            _db = db;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _db.Categories.OrderByDescending(a => a.ModifiedDate).ToPagedListAsync(page, SiteConst.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Form(int? id)
        {
            var category = new CategoryViewModel();

            if (id > 0)
            {
                var entity = await _db.Categories.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                category = entity.ToModel();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();

                if (id > 0)
                {

                    if (id != model.Id)
                    {
                        return NotFound();
                    }

                    _db.Categories.Update(entity);
                }
                else
                {
                    if (await _db.Categories.AnyAsync(a => EF.Functions.Like(a.Name, model.Name)))
                    {
                        TempData["error"] = _localizer.GetString("CategoryExist").Value;

                        return View(model);
                    }
                    else
                    {
                        _db.Categories.Add(entity);
                    }
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Сохранено";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _db.Categories.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;

            var category = _db.Categories.FirstOrDefault();
            if (category != null)
            {
                SetUncategorizedQuestions(entity.Id, category.Id);
            }

            _db.Categories.Update(entity);

            await _db.SaveChangesAsync();

            TempData["success"] = "Удалено";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadExcel(IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileStream = uploadedFile.OpenReadStream();
                using var workBook = new XLWorkbook(fileStream, XLEventTracking.Disabled);
                var workSheet = workBook.Worksheet(1);
                var firstRow = true;
                var categoryList = new List<Category>();

                foreach (var row in workSheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        firstRow = false;
                    }
                    else
                    {
                        var name = row.Cell(1).GetString();
                        var entity = _db.Categories.FirstOrDefault(e => e.Name == name);

                        if (entity == null)
                        {
                            categoryList.Add(new Category
                            {
                                Name = row.Cell(1).Value.ToString(),
                                Description = row.Cell(2).Value.ToString()
                            });
                        }
                    }
                }
                if (categoryList.Count > 0)
                {
                    _db.Categories.AddRange(categoryList);

                    await _db.SaveChangesAsync();
                }

                if (firstRow)
                {
                    TempData["error"] = _localizer.GetString("EmptyExcelFile").Value;
                }
            }
            else
            {
                TempData["error"] = _localizer.GetString("SelectXLSXFile").Value;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
