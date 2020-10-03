using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SharerBlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class FileDownloadController : ControllerBase
    {
        private readonly ILogger<FileDownloadController> _logger;
        private readonly string folderName = Path.Combine("Resources", "Download");

        public FileDownloadController(ILogger<FileDownloadController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetFileList()
        {
            Directory.CreateDirectory(this.folderName);
            string[] fullFilePaths = Directory.GetFiles(folderName);

            var filenames = new List<string>();
            foreach (string fullFilePath in fullFilePaths)
            {
                filenames.Add(Path.GetFileName(fullFilePath));
            }
            return Ok(filenames);
        }

        public IActionResult DownloadFile([FromQuery] string filename)
        {
            string filePath = Path.Combine(this.folderName, filename);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var content = System.IO.File.ReadAllBytes(filePath);
            return File(content, "application/octet-stream", filename);
        }
    }
}
