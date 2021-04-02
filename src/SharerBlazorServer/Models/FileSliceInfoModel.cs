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
                if (this.FileSize < 1024)
                {
                    return $"{this.FileSize:0} B";
                }
                else if (this.FileSize < 1024 * 1024)
                {
                    return $"{this.FileSize / 1024:0} KB";
                }
                else if (this.FileSize < 1024 * 1024 * 1024)
                {
                    return $"{(double)this.FileSize / (1024 * 1024):0.##} M";
                }
                else
                {
                    return $"{(double)this.FileSize / (1024 * 1024 * 1024):0.##} G";
                }
            }
        }
    }
}
