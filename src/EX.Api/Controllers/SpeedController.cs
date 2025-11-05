using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EX.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SpeedController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;

        public SpeedController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "tmp", $"{DateTime.Now:yyyyddMMHHmmss}");
           
            await using (var fileStream = System.IO.File.OpenWrite(filePath))
            {
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            await Task.Factory.StartNew(() =>
            {
                System.IO.File.Delete(filePath);
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Download()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "tmp", "uzex");
            var fileData = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileData, "application/octet-stream");
        }

    }
}