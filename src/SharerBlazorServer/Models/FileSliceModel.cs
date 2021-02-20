namespace SharerBlazorServer.Models
{
    public class FileSliceModel
    {
        public byte[] Piece { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Filename { get; set; }
        public int FileSize { get; set; }
        public string FileMd5 { get; set; }
    }
}
