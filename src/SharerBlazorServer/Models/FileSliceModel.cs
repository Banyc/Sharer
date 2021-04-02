using System.Numerics;
using System.Text.Json.Serialization;

namespace SharerBlazorServer.Models
{
    public class FileSliceModel : FileSliceInfoModel
    {
        public byte[] Piece { get; set; }

        [JsonIgnore]
        public bool IsIntegral {
            get
            {
                return new BigInteger(this.Piece).GetHashCode() == this.FilePieceHashCode;
            }
        }
    }
}
