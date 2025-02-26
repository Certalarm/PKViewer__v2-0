using PKInfo.Utility.Enum;
using System.Collections.Generic;

namespace PKInfo.Data.DTO
{
    internal class KeyMediaInfoDB
    {
        public string RootPath { get; set; }
        public string Label { get; set; }
        public long Size { get; set; }
        public KeyMediaType Type { get; set; }
        public string Error { get; set; } = string.Empty;
        public IList<KeyContainerTypeInfoDB> KeyContainerTypesInfoDB { get; set; } = [];
    }
}
