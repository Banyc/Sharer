using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SharerBlazorServer.Models;
using SharerBlazorServer.Helpers;

namespace SharerBlazorServer.Services
{
    public class ResumeFileSettings
    {
        public string SaveDirectory { get; set; }
    }

    public class ResumeFileService
    {
        private readonly ResumeFileSettings config;
        private readonly Dictionary<string, FilestreamWriter> filenameStream = new();

        public ResumeFileService(IOptions<ResumeFileSettings> options)
        {
            this.config = options.Value;
            Directory.CreateDirectory(options.Value.SaveDirectory);
        }

        public bool TryAddPiece(FileSliceModel slice)
        {
            lock (this)
            {
                if (!this.filenameStream.Keys.Contains(slice.Filename))
                {
                    string filePath = Path.Combine(this.config.SaveDirectory, slice.Filename);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    FileStream stream = File.OpenWrite(filePath);
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
