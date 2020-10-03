using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharerBlazorServer.Services;

namespace SharerBlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly ResumeFileService resumeFile;

        public FileUploadController(ILogger<FileUploadController> logger, ResumeFileService resumeFile)
        {
            this.resumeFile = resumeFile;
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult OnPost()
        {
            // return "good post";
            try
            {
                var additionalMessage = this.Request.Form["addition_text"];
                if (!string.IsNullOrEmpty(additionalMessage))
                {
                    _logger.LogInformation(additionalMessage);
                }
                if (Request.Form.Files.Count == 0)
                {
                    return Ok(new { additionalMessage });
                }
                if (this.Request.Form.ContainsKey("start"))
                {
                    object response = HandleFilePiece(this.Request.Form);
                    return Ok(response);
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

        private object HandleFilePiece(IFormCollection form)
        {
            int start = int.Parse(form["start"]);
            int end = int.Parse(form["end"]);
            int fileSize = int.Parse(form["fileSize"]);
            bool isFinal = bool.Parse(form["isFinal"]);
            IFormFile file = form.Files[0];

            string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                this.resumeFile.AddPiece(fileBytes, start, end, filename, fileSize, isFinal);
            }

            float progress = (float) end / fileSize;
            return new { Filename = filename, Range = $"{start}-{end}/{fileSize}", Progress = $"{progress}" };
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("good");
        }
    }
}
