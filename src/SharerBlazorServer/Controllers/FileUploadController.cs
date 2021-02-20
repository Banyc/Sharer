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
            this.resumeFile.AddPiece(fileSlice);

            float progress = (float) fileSlice.End / fileSlice.FileSize;
            return Ok(new { Filename = fileSlice.Filename, Range = $"{fileSlice.Start}-{fileSlice.End}/{fileSlice.FileSize}", Progress = $"{progress}" });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("good");
        }
    }
}
