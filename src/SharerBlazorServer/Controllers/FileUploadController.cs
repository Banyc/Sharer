using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SharerBlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(ILogger<FileUploadController> logger)
        {
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult OnPost()
        {
            // return "good post";
            try
            {
                var additionalMessage = this.Request.Form["addition_text"];
                _logger.LogInformation(additionalMessage);

                if (Request.Form.Files.Count == 0)
                {
                    return Ok(new { additionalMessage });
                }
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Upload");
                Directory.CreateDirectory(folderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("good");
        }
    }
}
