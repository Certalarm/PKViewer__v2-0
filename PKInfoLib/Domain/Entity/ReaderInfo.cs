using System.Collections.Generic;
using PKInfo.Utility.Enum;

namespace PKInfo.Domain.Entity
{
    internal sealed class ReaderInfo()
    {
        internal ReaderType Type { get; set; }
        internal string Error { get; set; }
        internal IList<KeyMediaInfo> KeyMediasInfo { get; set; } = [];
    }
}
