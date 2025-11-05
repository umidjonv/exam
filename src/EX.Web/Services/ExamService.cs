using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EX.Common;
using EX.Common.Rest;
using EX.Data.Core;
using EX.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EX.Web.Services
{
    public class ExamService : ApiClient
    {
        private readonly IAppDbContext _db;

        public ExamService(IAppDbContext db, AppConfig config) : base(config.ExamApi)
        {
            _db = db;
        }

        public async Task AddUserSession(string userId, string ip, string agent)
        {
            var user = await _db.Users.Where(a => a.Id == userId)
                .Include(a => a.Sessions)
                .FirstOrDefaultAsync();
            var session = new UserInSession
            {
                IpAddress = ip,
                UserAgent = agent,
                LoginTime = DateTime.Now
            };

            if (user == null)
            {
                _db.Users.Add(new User
                {
                    LastActivity = DateTime.Now,
                    Id = userId,
                    Sessions = new List<UserInSession>
                    {
                        session
                    }
                });
            }
            else
            {
                user.LastActivity = DateTime.Now;
                user.Sessions.Add(session);

                _db.Users.Update(user);
            }

            await _db.SaveChangesAsync();
        }

        public Task Listen(string ownerId)
        {
            return Post($"/exam/listen/{ownerId}", null);
        }

        public Task Assign(string ownerId)
        {
            return Post($"/exam/assign/{ownerId}", null);
        }

        public Task Finish(string ownerId)
        {
            return Post($"/exam/finish/{ownerId}", null);
        }

        public Task<IEnumerable<string>> Files(string ownerId, string sessionId)
        {
            return Post<IEnumerable<string>>($"/exam/files/{ownerId}?session={sessionId}", null);
        }

        public async Task<Stream> Download(string ownerId, string fileName)
        {
            var client = CreateHttpClient();
            var request = await client.PostAsync($"/exam/download/{ownerId}?file={fileName}", null);

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadAsStreamAsync();

            return Stream.Null;
        }

    }
}