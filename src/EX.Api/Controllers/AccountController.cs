using System;
using System.Threading.Tasks;
using EX.Api.Models;
using EX.Api.Services;
using EX.Common.Dtos;
using EX.Common.Rest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EX.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IdentityService _service; 

        public AccountController(IdentityService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(ApiResponse<UserTokenDto>))]
        public async Task<IActionResult> Token([FromBody] CredentialModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Модель недействительна");

            var result = await _service.LogIn(model.UserName, model.Password);

            return Ok(result);
        } 

        [HttpGet]
        [ProducesDefaultResponseType(typeof(ApiResponse<UserInfoDto>))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Info()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var info = await _service.GetInfo(token);

            return Ok(info);
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(ApiResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logoff()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            await _service.LogOut(token);

            return Ok();
        }

    }
}
