using System;

namespace PKInfo.Data.DTO
{
    internal class FileMetaInfo(long size, DateTime? lastWriteTimeUTC)
    {
        public long Size => size;
        public DateTime? LastWriteTimeUTC => lastWriteTimeUTC;
    }
}
