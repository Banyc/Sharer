using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SharerBlazorServer.Models;
using SharerBlazorServer.Helpers;
using Microsoft.Extensions.Logging;

namespace SharerBlazorServer.Services
{
    public class ResumeFileSettings
    {
        public string SaveDirectory { get; set; }
    }

    public class ResumeFileService
    {
        private readonly ILogger<ResumeFileService> _logger;
        private readonly ResumeFileSettings config;
        private readonly Dictionary<string, FilestreamWriter> filenameStream = new();

        public ResumeFileService(IOptions<ResumeFileSettings> options, ILogger<ResumeFileService> logger)
        {
            this.config = options.Value;
            _logger = logger;
            Directory.CreateDirectory(options.Value.SaveDirectory);
        }

        public bool TryAddPiece(FileSliceModel slice)
        {
            lock (this)
            {
                if (!this.filenameStream.Keys.Contains(slice.Filename))
                {
                    // determine file path
                    string filePath;
                    if (slice.Filename.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                    {
                        // valid file name
                        filePath = Path.Combine(this.config.SaveDirectory, slice.Filename);
                    }
                    else
                    {
                        // invalid file name
                        filePath = Path.Combine(this.config.SaveDirectory,
                            slice.Filename.GetHashCode().ToString());
                    }
                    // rewrite file if exists
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    // try build file stream
                    FileStream stream;
                    try
                    {
                        stream = File.OpenWrite(filePath);
                    }
                    catch (PathTooLongException)
                    {
                        // invalid file name
                        filePath = Path.Combine(this.config.SaveDirectory,
                            slice.Filename.GetHashCode().ToString());
                        stream = File.OpenWrite(filePath);
                    }
                    // save the file stream for further use
                    this.filenameStream[slice.Filename] = new(stream);
                    this.filenameStream[slice.Filename].OnComplete += this.OnComplete;
                }
            }

            return this.filenameStream[slice.Filename].TryAddPiece(slice);
        }

        private void OnComplete(FilestreamWriter sender)
        {
            lock (this)
            {
                this.filenameStream.Remove(sender.Filename);
            }
        }
    }
}
