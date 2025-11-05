using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Web.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EX.Web.Areas.Client.Controllers
{
    public class ResultController : BaseMvcController
    {
        private readonly IAppDbContext _db;

        public ResultController(IAppDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _db.Users.FindAsync(UserId);
            var model = await _db.ExamResults
                .Where(e => e.UserId == user.Id)
                .Include(e => e.User)
                .Include(e => e.Exam)
                .Include(e => e.Exam.Options)
                .Include(e => e.Units)
                .ThenInclude(x => x.Answer)
                .ThenInclude(x => x.Corrects)
                .Include(e => e.Schedule)
                .OrderByDescending(x => x.StartTime)
                .ToPagedListAsync(page, SiteConst.PageSize);


            return View(model);
        }


    }
}