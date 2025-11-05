using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EX.Api.Dtos;
using EX.Api.Hubs;
using EX.Api.Services;
using EX.Common.Dtos;
using EX.Common.Extensions;
using EX.Common.Rest;
using EX.Data.Core;
using EX.Data.Entities;
using EX.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UzEx.Storage.MinIO;

namespace EX.Api.Controllers
{
    public class ExamController : BaseApiController
    {
        private readonly ExamService _service;
        private readonly IHubContext<StreamHub> _streamHub;
        private readonly IHubContext<ExamHub> _examHub;
        private readonly IAppDbContext _db;
        private readonly MinStorageClient _store;

        private async Task PushExam(string owner)
        {
            var model = await GetLastExam(owner);

            await _examHub.Clients.All.SendAsync($"Assign-{owner}", model); 
        }

        private async Task<ExamStateDto> GetLastExam(string ownerId)
        {
            var exam = await _service.Last(ownerId);

            if (exam == null)
            {
                throw new Exception("У вас нету активных экзаменов");
            }

            var dateNow = DateTime.Now;
            var startDate = exam.Schedule.StartDate;
            var duration = exam.Exam.DurationInMinutes;
            var endTime = startDate.AddMinutes(duration);

            if (endTime < dateNow)
            {
                throw new Exception("Истёк срок по вашим экзаменам");
            }

            return new ExamStateDto
            {
                StartTime = startDate.ConvertFullDate(),
                EndTime = endTime.ConvertFullDate()
            };
        }

        private async Task<ExamStateDto> GetActiveExam(string owner)
        {
            var exam = await _service.Active(owner);

            if (exam == null)
            {
                throw new Exception("У вас нету активных экзаменов");
            }

            var dateNow = DateTime.Now;
            var startDate = exam.Schedule.StartDate;
            var duration = exam.Exam.DurationInMinutes;
            var endTime = startDate.AddMinutes(duration);

            if (endTime < dateNow)
            {
                throw new Exception("Истёк срок по вашим экзаменам");
            }

            return new ExamStateDto
            {
                StartTime = startDate.ConvertFullDate(),
                EndTime = endTime.ConvertFullDate()
            };
        }

        public ExamController(ExamService service, IHubContext<StreamHub> streamHub, IAppDbContext db,
            MinStorageClient store, IHubContext<ExamHub> examHub)
        {
            _service = service;
            _streamHub = streamHub;
            _db = db;
            _store = store;
            _examHub = examHub;
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> IsPassed([FromRoute] string owner)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            var exam = await _db.UserInExams.Where(a => a.UserId == owner)
                .OrderByDescending(a => a.PassedTime)
                .FirstOrDefaultAsync(a => a.IsPassed == true);

            if (exam == null)
                throw new Exception("Вы еще не сдали экзамен");

            return Ok();
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse<ExamStateDto>))]
        public async Task<IActionResult> CheckStatus([FromRoute] string owner)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            var model = await GetActiveExam(owner);

            return Ok(model);
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Listen([FromRoute] string owner)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            await _streamHub.Clients.All.SendAsync($"Listen-{owner}");

            return Ok();
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Assign([FromRoute] string owner)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            await PushExam(owner);

            return Ok();
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Finish([FromRoute] string owner)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            await _examHub.Clients.All.SendAsync($"Finish-{owner}", true);

            return Ok();
        }

        [HttpPost("{owner}")]
        [DisableRequestSizeLimit]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Upload([FromRoute] string owner, [FromQuery] string session, [FromForm] IFormFile file)
        {
            var client = await _db.UserInClients.FirstOrDefaultAsync(a => a.UserSession == session && a.UserId == owner);

            if (client == null)
                throw new Exception("Клиент не найден");

            var stream = file.OpenReadStream();

            await _store.Upload(owner, $"stream-{session}-{DateTime.Now:yyyyddMMHHmmss}.mp4", stream, "video/mp4");

            return Ok();
        }

        [HttpPost("{owner}")]
        public async Task<IActionResult> Download([FromRoute] string owner, [FromQuery] string file)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            var stream = await _store.Download(owner, file);

            return File(stream, "video/mp4", file);
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse<IEnumerable<string>>))]
        public async Task<IActionResult> Files([FromRoute] string owner, [FromQuery] string session)
        {
            var client = await _db.UserInClients.FirstOrDefaultAsync(a => a.UserSession == session && a.UserId == owner);

            if (client == null)
                throw new Exception("Клиент не найден");

            var files = _store.Search(owner, $"stream-{session}");

            return Ok(files);
        }

        [HttpGet("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse<IEnumerable<ExamDto>>))]
        public async Task<IActionResult> Filter([FromRoute] string owner, [FromQuery] int region, [FromQuery] bool own)
        {
            IQueryable<Schedule> query;

            if (own)
            {
                query = _db.Users.Where(x => x.Id == owner)
                    .Include(x => x.Exams)
                    .ThenInclude(a => a.Schedule)
                    .SelectMany(a => a.Exams.Select(b => b.Schedule));
            }
            else
            {
                query = _db.Schedules.Where(x => x.Status != ScheduleStatus.Finished)
                    .Include(x => x.Exam)
                    .Include(x => x.Users);
            }

            if (region > 0)
                query = query.Where(w => w.RegionId == region);

            if (!await query.AnyAsync())
                throw new Exception("Нет активных экзаменов!");

            var exams = query.OrderByDescending(a => a.StartDate)
                .Select(a => new ExamDto
                {
                    Id = a.ExamId,
                    ScheduleId = a.Id,
                    Mode = $"{a.Mode}",
                    Code = a.Exam.Code,
                    Name = a.Exam.Title,
                    StartDate = a.StartDate,
                    Participants = $"{a.LimitOfParticipants} / {a.Users.Count(x => x.IsPassed != true)}",
                    Status = $"{a.Status}",
                    IsEnrolled = a.Users.Any(b => b.UserId == owner && b.IsPassed != true)
                });

            return Ok(exams);
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Enroll([FromRoute] string owner, [FromQuery] int? schedule)
        {

            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            var entity = await _db.Schedules
                .Where(x => x.Id == schedule)
                .Include(x => x.Users)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new Exception("Расписание не найдено");

            var request = entity.Users.FirstOrDefault(a => a.UserId == owner && a.IsPassed == false);
            var countOfParticipants = entity.Users.Count(x => x.IsAdmit);

            if (entity.LimitOfParticipants <= countOfParticipants)
            {
                throw new Exception("Количество участников экзамена закончилось!");
            }

            var exam = await _db.UserInExams
                .Where(x => x.ExamId == entity.ExamId && x.UserId == user.Id && x.IsPassed == true)
                .Include(x => x.Schedule)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync();

            if (exam != null)
            {
                var period = entity.StartDate
                    .Subtract(exam.Schedule.StartDate.AddDays(exam.Exam.PeriodInDays)).Days;

                if (period < 0)
                {
                    throw new Exception($"Время следующего экзамена{Math.Abs(period)}");
                }
            }

            if (request == null)
            {
                _db.UserInExams.Add(new UserInExams
                {
                    ScheduleId = entity.Id,
                    IsPassed = false,
                    RequestTime = DateTime.Now,
                    UserId = user.Id,
                    IsAdmit = true,
                    ExamId = entity.ExamId
                });

                await _db.SaveChangesAsync();

                if (entity.Mode == ExamMode.Online)
                {
                    await PushExam(owner);
                }
            }

            return Ok();
        }

        [HttpPost("{owner}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> DisEnroll([FromRoute] string owner, [FromQuery] int schedule)
        {
            var user = await _db.Users.FindAsync(owner);

            if (user == null)
                throw new Exception("Пользователь не найден");

            var entity = await _db.Schedules
                .Where(x => x.Id == schedule)
                .Include(x => x.Users)
                .Include(x => x.Exam)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new Exception("Расписание не найдено");

            var exam = await _db.UserInExams.FirstOrDefaultAsync(x => x.IsAdmit && x.ScheduleId == schedule && x.UserId == user.Id);

            if (exam != null)
            {
                _db.UserInExams.Remove(exam);

                await _db.SaveChangesAsync();
            }

            return Ok();
        }

    }
}
