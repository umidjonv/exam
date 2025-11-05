using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EX.Data.Core;
using X.PagedList;
using EX.Data.Entities;
using EX.Web.Consts;
using Microsoft.AspNetCore.Http;
using UzEx.Storage.MinIO;

namespace EX.Web.Areas.Admin.Controllers
{
    public class DocumentController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly MinStorageClient _store;
        private const string FILE_PREFIX = "doc";
        private const string FILES_ROOT = "documents";

        private async Task UploadFile(IFormFile file, long id)
        {
            var stream = file.OpenReadStream();

            await _store.Upload(FILES_ROOT, $"{FILE_PREFIX}{id}-{file.FileName}", stream, file.ContentType);
        }

        public DocumentController(IAppDbContext db, MinStorageClient store)
        {
            _db = db;
            _store = store;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _db.Documents.OrderByDescending(a => a.ModifiedDate).ToPagedListAsync(page, SiteConst.PageSize);

            return View(model);
        }

        public async Task<IActionResult> Form(int? id)
        {
            var entity = new Document();

            if (id > 0)
            {
                entity = await _db.Documents.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Form(int? id, Document model)
        {
            if (ModelState.IsValid)
            {
                var entity = model;
                var file = Request.Form.Files["file1"]; 

                if (id > 0)
                {
                    if (id != model.Id)
                    {
                        return NotFound();
                    }

                    entity = await _db.Documents.FindAsync(id);
                    
                    if (file != null)
                    {
                        entity.FileName = file.FileName;
                    }

                    entity.Description = model.Description;
                    entity.Title = model.Title;

                    _db.Documents.Update(entity);
                }
                else
                {
                    if (file != null)
                    {
                        entity.FileName = file.FileName;
                    }

                    _db.Documents.Add(entity);
                }

                await _db.SaveChangesAsync();

                if (file != null)
                {
                    await UploadFile(file, entity.Id);
                }

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

            var entity = await _db.Documents.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;

            _db.Documents.Update(entity);

            await _db.SaveChangesAsync();

            await _store.Delete(FILES_ROOT, $"{FILE_PREFIX}{id}-{entity.FileName}");

            TempData["success"] = "Удалено";

            return RedirectToAction(nameof(Index));
        }

    }
}
