namespace SharerBlazorServer.Models
{
    public class FileSliceInfoModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Filename { get; set; }
        public int FileSize { get; set; }
        public string FileMd5 { get; set; }
        public double Process {
            get => ((double)this.End) / this.FileSize;
        }
    }
}
