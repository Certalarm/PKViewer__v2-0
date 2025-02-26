using PKInfo.Data.DTO;
using PKInfo.Data.Implementation.DiskKeyMediaReader;
using PKInfo.Data.Implementation.DiskReader;
using PKInfo.Data.Interface;
using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PKInfo.Utility.Enum;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation
{
    internal abstract class BaseDiskReader : IInfoReaderDB<ReaderInfoDB>
    {
        private const int MAX_FLOPPY_DRIVE_SIZE = 1_500_000;

        private readonly ReaderType _type;
        protected IList<DriveInfo> _drivesInfo = [];
        protected bool _needDeleted;
        protected string _error = string.Empty;

        #region .ctors
        protected BaseDiskReader(IEnumerable<DriveInfo> drivesInfo, ReaderType readerType, bool needDeleted)
        {
            _type = readerType;
            _needDeleted = needDeleted;
            if (drivesInfo == default)
                TryAddDrivesInfo();
            else _drivesInfo = [.. drivesInfo];
        }
        #endregion

        public virtual ReaderInfoDB Read()
        {
            var result = new ReaderInfoDB()
            {
                Type = _type,
            };
            if (!HasDrives(out IList<DriveMetaInfo> drivesMetaInfo, out string error))
                result.Error = error;
            foreach(var kmi in GetKeyMediasInfo(drivesMetaInfo))
                result.KeyMeadiasInfoDB.Add(kmi);
            return result;
        }

        private IList<KeyMediaInfoDB> GetKeyMediasInfo(IList<DriveMetaInfo> drivesMetaInfo)
        {
            IList<KeyMediaInfoDB> result = [];
            foreach (var metaInfo in drivesMetaInfo)
            {
                var kmiReader = GetReader(metaInfo);
                if (kmiReader is null) continue;
                var kmi = metaInfo.Error.Length < 1
                    ? kmiReader.Read()
                    : InitKmiIfError(metaInfo);
                if (kmi is not null)
                    result.Add(kmi);
            }
            return result;
        }

        private IInfoReaderDB<KeyMediaInfoDB> GetReader(DriveMetaInfo metaInfo) =>
            _type switch
            {
                Utility.Enum.ReaderType.AllRemovable => GetRemovableReader(metaInfo),
                Utility.Enum.ReaderType.AllFixed => new LocalDiskReader(metaInfo, _needDeleted),
                _ => null
            };

        private IInfoReaderDB<KeyMediaInfoDB> GetRemovableReader(DriveMetaInfo metaInfo) =>
            IsFloppyDrive(metaInfo)
                ? new FloppyDiskReader(metaInfo, _needDeleted)
                : new FlashDiskReader(metaInfo, _needDeleted);

        private bool IsFloppyDrive(DriveMetaInfo metaInfo) =>
            metaInfo.Size < MAX_FLOPPY_DRIVE_SIZE;

        private bool HasDrives(out IList<DriveMetaInfo> drivesMetaInfo, out string error)
        {
            drivesMetaInfo = GetDrivesMetaInfo();
            error = _error;
            return _error.Length == 0 && drivesMetaInfo.Any(x => x.Size > 0);
        }

        private IList<DriveMetaInfo> GetDrivesMetaInfo()
        {
            IList<DriveMetaInfo> drivesMetaInfo = [];
            if (!string.IsNullOrEmpty(_error))
                return drivesMetaInfo;
            _error = FilterDrivesInfo();
            return string.IsNullOrEmpty(_error)
                ? _drivesInfo.AsDrivesMetaInfo()
                : drivesMetaInfo; 
        }

        private string FilterDrivesInfo()
        {
            if (!_drivesInfo.Any())
                return DrivesNotFound;
            _drivesInfo = _drivesInfo.GetDrivesByType(_type);
            if (!_drivesInfo.Any())
                return DrivesTypeNotFound;
            if (!_drivesInfo.Any())
                return DrivesReadyNotFound;
            return string.Empty;
        }

        private void TryAddDrivesInfo()
        {
            try
            {
                _drivesInfo = DriveInfo.GetDrives();
            }
            catch (Exception ex)
            {
                _error = ex.AsError();
            }
        }

        private KeyMediaInfoDB InitKmiIfError(DriveMetaInfo metaInfo) =>
            new ()
            { 
                Error = metaInfo.Error, 
                RootPath = metaInfo.Name 
            };
    }
}