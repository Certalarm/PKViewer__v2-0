﻿using System.Collections.Generic;
using System.IO;
using PKInfo.Utility.Enum;

namespace PKInfo.Data.Implementation.DiskReader
{
    internal class RemovableDiskReader(bool needDeleted, IEnumerable<DriveInfo> drivesInfo = default) 
        : BaseDiskReader(drivesInfo, ReaderType.AllRemovable, needDeleted)
    {
    }
}
