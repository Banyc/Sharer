using System;
using System.IO;
using SharerBlazorServer.Models;

namespace SharerBlazorServer.Helpers
{
    public class FilestreamWriter
    {
        public event Action<FilestreamWriter> OnComplete;
        public string FilePath { get; }
        public string Filename { get; }
        private readonly FileStream stream;
        public FilestreamWriter(FileStream stream)
        {
            this.stream = stream;
            this.FilePath = stream.Name;
            this.Filename = Path.GetFileName(stream.Name);
        }

        public bool TryAddPiece(FileSliceModel slice)
        {
            if (this.stream.Position > slice.Start)
            {
                return false;
            }
            this.stream.Position = slice.Start;
            this.stream.Write(slice.Piece, 0, slice.End - slice.Start);

            if (slice.FileSize == slice.End)
            {
                this.stream.Close();
                this.OnComplete?.Invoke(this);
            }

            return true;
        }
    }
}
