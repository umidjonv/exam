using System;
using System.Threading.Tasks;
using EX.Api.Models;
using EX.Common.Rest;
using EX.Data.Core;
using EX.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EX.Api.Controllers
{
    public class SessionController : BaseApiController
    {
        private readonly IAppDbContext _db;

        public SessionController(IAppDbContext db)
        {
            _db = db;
        }

        [HttpPost("{session}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Set([FromRoute] string session, [FromBody] SetSessionModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Модель недействительна");

            var user = await _db.Users.FindAsync(UserId);

            if (user == null)
                throw new Exception("Пользователь не найден");

            _db.UserInClients.Add(new UserInClient
            {
                LoginTime = DateTime.Now,
                UserSession = session,
                IpAddress = model.IpAddress,
                ComputerDomain = model.ComputerDomain,
                ComputerName = model.ComputerName,
                ComputerUser = model.ComputerUser,
                UserId = user.Id
            });

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{session}")]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        public async Task<IActionResult> Active([FromRoute] string session, [FromBody] ActiveSessionModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Модель недействительна");

            var client = await _db.UserInClients.FirstOrDefaultAsync(a => a.UserSession == session);
          
            if (client == null)
                throw new Exception("Клиент не найден");

            client.AudioDriver = model.AudioDriver;
            client.VideoDriver = model.VideoDriver;
            client.IsConnected = true;
            client.ConnectTime = DateTime.Now;

            _db.UserInClients.Update(client);

            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}