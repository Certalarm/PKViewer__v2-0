using PKInfo.Data.DTO;
using PKInfo.Utility;
using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PKInfo.Data.Implementation.DiskReader
{
    internal static class BaseDiskReaderExts
    {
        internal static IList<DriveMetaInfo> AsDrivesMetaInfo(this IEnumerable<DriveInfo> drivesInfo) =>
            drivesInfo
                .Select(GetDriveMetaInfo)
                .ToList();

        internal static IList<DriveInfo> GetDrivesByType(this IEnumerable<DriveInfo> drivesInfo, ReaderType readerType) =>
            drivesInfo
                .Where(x => x.DriveType == readerType.AsDriveType())
                .Where(x => x.DriveType != DriveType.Unknown)
                .ToList();

        private static DriveMetaInfo GetDriveMetaInfo(DriveInfo driveInfo)
        {
            var error = TryGetDriveMetaInfo(driveInfo, out var metaInfo);
            return error.Length > 0
                ? new DriveMetaInfo(driveInfo.Name, string.Empty, 0, error)
                : metaInfo;
        }

        private static string TryGetDriveMetaInfo(DriveInfo info, out DriveMetaInfo metaInfo)
        {
            metaInfo = null;
            if (!info.IsReady)
                return Txt.DriveNotReady;
            try
            {
                metaInfo = new(info.Name, info.VolumeLabel, info.TotalSize, string.Empty);
            }
            catch (Exception ex)
            {
                return ex.AsError();
            }
            return string.Empty;
        }

        private static DriveType AsDriveType(this ReaderType readerType) =>
             readerType switch
             {
                 ReaderType.AllRemovable => DriveType.Removable,
                 ReaderType.AllFixed => DriveType.Fixed,
                 // Add here other known types 
                 _ => DriveType.Unknown
             };
    }
}
