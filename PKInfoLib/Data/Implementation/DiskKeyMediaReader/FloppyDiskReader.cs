using PKInfo.Data.DTO;
using PKInfo.Utility.Enum;

namespace PKInfo.Data.Implementation.DiskKeyMediaReader
{
    internal class FloppyDiskReader(DriveMetaInfo metaInfo, bool needDeleted)
        : BaseDiskKeyMediaReader(metaInfo, KeyMediaType.FloppyDrive, needDeleted)
    {
    }
}
