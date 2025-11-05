using System;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using EX.Data.Core;
using EX.Data.Enums;
using EX.Web.Consts;
using EX.Web.Mappers;
using EX.Web.Models;
using EX.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EX.Web.Areas.Admin.Controllers
{
    public class ScheduleController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly HandbookService _handbook;
        private readonly ExamService _service;

        private async Task LoadViewData(ScheduleViewModel model)
        {
            ViewBag.RegionList = new SelectList(await _handbook.GetRegions(), "Id", "Name", model.RegionId);
            ViewBag.ExamList = new SelectList(await _db.Exams.ToArrayAsync(), "Id", "Title", model.ExamId);
        }

        public ScheduleController(IAppDbContext db, HandbookService handbook, ExamService service)
        {
            _db = db;
            _handbook = handbook;
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _db.Schedules
                .Include(a => a.Exam)
                .OrderByDescending(x => x.StartDate)
                .ToPagedListAsync(page, SiteConst.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Form(int? id)
        {
            var model = new ScheduleViewModel();

            if (id > 0)
            {
                var entity = await _db.Schedules.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                model = entity.ToModel();
            }
            else
            {
                model.StartDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }

            await LoadViewData(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, ScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewData(model);

                return View(model);
            }

            var entity = model.ToEntity();

            if (id > 0)
            {

                if (id != model.Id)
                {
                    return NotFound();
                }
                entity.Status = ScheduleStatus.New;

                _db.Schedules.Update(entity);

                await _db.SaveChangesAsync();

                #region Notify desk for avaliable clients

                var exams = await _db.UserInExams.Where(w => w.ScheduleId == id && w.IsAdmit && w.IsPassed == false)
                    .Include(a => a.Schedule)
                    .Where(w => w.Schedule.Mode == ExamMode.Online)
                    .ToArrayAsync();

                foreach (var exam in exams)
                {
                    await _service.Assign(exam.UserId);
                }

                #endregion

            }
            else
            {
                _db.Schedules.Add(entity);

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

            var entity = await _db.Schedules.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;

            _db.Schedules.Update(entity);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
