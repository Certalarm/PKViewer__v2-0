using PKInfo.Data.DTO;
using PKInfo.Utility.Enum;

namespace PKInfo.Data.Implementation.DiskKeyMediaReader
{
    internal class FlashDiskReader(DriveMetaInfo metaInfo, bool needDeleted)
        : BaseDiskKeyMediaReader(metaInfo, KeyMediaType.FlashDrive, needDeleted)
    {
    }
}
