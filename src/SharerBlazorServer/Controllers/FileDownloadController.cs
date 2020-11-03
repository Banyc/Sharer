using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SharerBlazorServer.Controllers
{
    public class FileDownloadControllerSettings
    {
        public string SearchDirectory { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class FileDownloadController : ControllerBase
    {
        private readonly ILogger<FileDownloadController> _logger;
        private readonly FileDownloadControllerSettings config;

        public FileDownloadController(ILogger<FileDownloadController> logger, IOptions<FileDownloadControllerSettings> options)
        {
            _logger = logger;
            this.config = options.Value;
        }

        [HttpGet]
        public IActionResult GetFileList()
        {
            Directory.CreateDirectory(this.config.SearchDirectory);
            string[] fullFilePaths = Directory.GetFiles(this.config.SearchDirectory);

            var filenames = new List<string>();
            foreach (string fullFilePath in fullFilePaths)
            {
                filenames.Add(Path.GetFileName(fullFilePath));
            }
            return Ok(filenames);
        }

        public IActionResult DownloadFile([FromQuery] string filename)
        {
            string filePath = Path.Combine(this.config.SearchDirectory, filename);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            // do NOT prepend a `using` keyword, which will dispose the stream before the file is downloaded
            FileStream fileStream = System.IO.File.OpenRead(filePath);

            return File(fileStream, "application/octet-stream", filename);
        }
    }
}
