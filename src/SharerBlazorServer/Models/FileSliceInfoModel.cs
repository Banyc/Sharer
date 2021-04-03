using System.Text.Json.Serialization;

namespace SharerBlazorServer.Models
{
    public class FileSliceInfoModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Filename { get; set; }
        public int FileSize { get; set; }
        public int FilePieceHashCode { get; set; }

        [JsonIgnore]
        public double Process {
            get => ((double)this.End) / this.FileSize;
        }
        [JsonIgnore]
        public string FileSizeHumanReadable {
            get
            {
                return GetHumanReadableSize(this.FileSize);
            }
        }

        [JsonIgnore]
        public string EndHumanReadable
        {
            get
            {
                return GetHumanReadableSize(this.End);
            }
        }

        private static string GetHumanReadableSize(int byteSize)
        {
            if (byteSize < 1024)
            {
                return $"{byteSize:0} B";
            }
            else if (byteSize < 1024 * 1024)
            {
                return $"{byteSize / 1024:0} KB";
            }
            else if (byteSize < 1024 * 1024 * 1024)
            {
                return $"{(double)byteSize / (1024 * 1024):0.##} M";
            }
            else
            {
                return $"{(double)byteSize / (1024 * 1024 * 1024):0.##} G";
            }
        }
    }
}
