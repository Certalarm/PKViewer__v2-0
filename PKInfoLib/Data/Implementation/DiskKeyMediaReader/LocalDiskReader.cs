using PKInfo.Data.DTO;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System;
using System.IO;
using static PKInfo.Utility.Exts;

namespace PKInfo.Data.Implementation.DiskKeyMediaReader
{
    internal class LocalDiskReader(DriveMetaInfo metaInfo, bool needDeleted) 
        : BaseDiskKeyMediaReader(metaInfo, KeyMediaType.LocalDrive, needDeleted)
    {
        public override KeyMediaInfoDB Read() =>
            IsSystemDrive()
                ? ReadIfSystemDrive()
                : base.Read();

        private KeyMediaInfoDB ReadIfSystemDrive()
        {
            _error = Txt.SystemDrive;
            return GetKeyMediaWithError();
        }

        private bool IsSystemDrive() =>
           _metaInfo.Name.EqualsIgnoreCase(GetSystemPathRoot());

        private static string GetSystemPathRoot()
        {
            string sysDir = Environment.SystemDirectory; 
            return Path.GetPathRoot(sysDir); 
        }

        private KeyMediaInfoDB GetKeyMediaWithError() =>
            new()
            {
                RootPath = _metaInfo.Name,
                Label = _metaInfo.Label,
                Size = _metaInfo.Size,
                Type = _type,
                Error = _error,
            };
    }
}
