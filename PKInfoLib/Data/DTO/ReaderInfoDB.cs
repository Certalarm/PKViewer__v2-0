using System.Collections.Generic;
using PKInfo.Utility.Enum;

namespace PKInfo.Data.DTO
{
    internal class ReaderInfoDB
    {
        public ReaderType Type { get; set; }
        public string Error { get; set; } = string.Empty;
        public IList<KeyMediaInfoDB> KeyMeadiasInfoDB { get; set; } = [];
    }
}
