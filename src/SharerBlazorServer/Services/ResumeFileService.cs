using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace SharerBlazorServer.Services
{
    public class ResumeFileSettings
    {
        public string SaveDirectory { get; set; }
    }

    public class ResumeFileService
    {
        private readonly ResumeFileSettings config;
        private readonly Dictionary<string, Stream> filenameStream = new Dictionary<string, Stream>();

        public ResumeFileService(IOptions<ResumeFileSettings> options)
        {
            this.config = options.Value;
            Directory.CreateDirectory(options.Value.SaveDirectory);
        }

        public void AddPiece(byte[] piece, int start, int end, string filename, int fileSize, bool isFinal)
        {
            if (!this.filenameStream.Keys.Contains(filename))
            {
                string filePath = Path.Combine(this.config.SaveDirectory, filename);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileStream stream = File.OpenWrite(filePath);
                this.filenameStream[filename] = stream;
            }

            this.filenameStream[filename].Position = start;
            this.filenameStream[filename].Write(piece, 0, end - start);

            if (isFinal)
            {
                this.filenameStream[filename].Close();
                this.filenameStream.Remove(filename);
            }
        }
    }
}
