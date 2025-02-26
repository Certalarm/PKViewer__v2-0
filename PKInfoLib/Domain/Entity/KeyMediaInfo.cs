using PKInfo.Utility.Enum;
using System.Collections.Generic;

namespace PKInfo.Domain.Entity
{
    internal sealed class KeyMediaInfo
    {
        internal string RootPath { get; set; }
        internal string Label { get; set; }
        internal long Size { get; set; }
        internal KeyMediaType Type { get; set; }
        internal string Error { get; set; }
        internal IList<ErrorKeyContainerTypeInfo> ErrorsKeyContainerTypes { get; set; } = [];
        internal IList<KeyContainerInfo> KeyContainersInfo { get; set; } = [];

        #region .ctors
        internal KeyMediaInfo()
        {
        }
        #endregion
    }
}
