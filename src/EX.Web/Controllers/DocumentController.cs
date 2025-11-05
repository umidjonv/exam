using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;
using UzEx.Storage.MinIO;
using X.PagedList;

namespace EX.Web.Controllers
{
    public class DocumentController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly MinStorageClient _store;
        private const string FILE_PREFIX = "doc";
        private const string FILES_ROOT = "documents";

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
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Download(int? id)
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

            var stream = await _store.Download(FILES_ROOT, $"{FILE_PREFIX}{id}-{entity.FileName}");

            return File(stream, "application/octet-stream", entity.FileName);
        }

    }
}