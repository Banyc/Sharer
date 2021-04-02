using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharerBlazorServer.Models;
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

        [HttpPost("filepiece"), DisableRequestSizeLimit]
        public IActionResult HandleFilePiece(FileSliceModel fileSlice)
        {
            if (fileSlice.IsIntegral)
            {
                bool isSucceeded = this.resumeFile.TryAddPiece(fileSlice);
                if (isSucceeded)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("text")]
        public IActionResult HandleText(TextModel text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text.Text);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========");
            Console.ForegroundColor = ConsoleColor.White;
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("good");
        }
    }
}
