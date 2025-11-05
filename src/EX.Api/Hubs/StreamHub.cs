using System;
using System.Linq;
using System.Threading.Tasks;
using EX.Data.Core;
using EX.Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EX.Api.Hubs
{

    public class StreamHub : Hub
    {

        private readonly IAppDbContext _db;

        private async Task<UserInClient> CheckAuth()
        {

            var userId = User;
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out _))
            {
                throw new Exception("User is null");
            }

            var sessionId = Session;
            if (string.IsNullOrWhiteSpace(sessionId) || !Guid.TryParse(sessionId, out _))
            {
                throw new Exception("Session is null");
            }

            var client = await _db.UserInClients.Where(w => w.UserSession == sessionId && w.UserId == userId)
                .Include(a => a.User)
                .FirstOrDefaultAsync();
            if (client == null)
            {
                throw new Exception("Client is null");
            }

            return client;
        }

        private string Session
        {
            get
            {
                var context = Context.GetHttpContext();

                return context.Request.Headers.ContainsKey("session")
                    ? context.Request.Headers["session"].ToString()
                    : "";
            }
        }

        private string User
        {
            get
            {
                var context = Context.GetHttpContext();

                return context.Request.Headers.ContainsKey("user")
                    ? context.Request.Headers["user"].ToString()
                    : "";
            }
        }

        public StreamHub(IAppDbContext db)
        {
            _db = db;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var client = await CheckAuth();

            if (exception != null)
            {
                var message = exception.InnerException?.Message ?? exception.Message;

                client.ErrorMessage = $"Client #{Context.ConnectionId}: {message}";
            }
            else
            {
                client.ErrorMessage = $"Client #{Context.ConnectionId}: Disconnected";
            }

            client.DisconnectTime = DateTime.Now;
            client.IsConnected = false;

            _db.UserInClients.Update(client);

            await _db.SaveChangesAsync();
        }

    }
}