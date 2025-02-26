using PKInfo.Utility.Enum;
using System.Collections.Generic;

namespace PKInfo.Data.DTO
{
    internal class KeyContainerTypeInfoDB
    {
        public KeyContainerType Type { get; set; }
        public string Error { get; set; } = string.Empty;
        public string ErrorDeleted { get; set; } = string.Empty;
        public IList<KeyContainerInfoDB> KeyContainersInfoDB { get; set; } = [];
        public IList<KeyContainerInfoDB> KeyContainersInfoDeletedDB { get; set; } = [];
    }
}
