using EX.Data.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Linq;
using EX.Web.Services;
using System.Collections.Generic;
using EX.Data.Entities;
using EX.Web.Areas.Admin.Models;
using EX.Web.Consts;
using EX.Web.Dtos;

namespace EX.Web.Areas.Admin.Controllers
{
    public class ReportController : BaseMvcController
    {
        private readonly IAppDbContext _db;
        private readonly IdentityService _identity;

        public ReportController(IAppDbContext db, IdentityService identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _db.ExamResults
                 .Include(e => e.Exam)
                 .Include(e => e.User)
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
    }
}