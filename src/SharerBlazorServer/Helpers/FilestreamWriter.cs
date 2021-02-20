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
        private int byteReceivedSoFar = 0;
        public FilestreamWriter(FileStream stream)
        {
            this.stream = stream;
            this.FilePath = stream.Name;
            this.Filename = Path.GetFileName(stream.Name);
        }

        public void AddPiece(FileSliceModel slice)
        {
            this.stream.Position = slice.Start;
            this.stream.Write(slice.Piece, 0, slice.End - slice.Start);

            this.byteReceivedSoFar += slice.End - slice.Start;

            if (this.byteReceivedSoFar == slice.FileSize)
            {
                this.stream.Close();
                this.OnComplete?.Invoke(this);
            }
        }
    }
}
