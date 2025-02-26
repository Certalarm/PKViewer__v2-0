using PKInfo.Data.DTO;
using PKInfo.Data.Implementation.DiskKeyContainerReader;
using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.CryptoProContainerHelper;

namespace PKInfo.Data.Implementation.DiskKeyContainerTypeReader
{
    internal class CryptoProKeyContainerTypeReader : BaseDiskKeyContainerTypeReader
    {
        #region .ctors
        public CryptoProKeyContainerTypeReader(string path, string deletedPath, bool needDeleted) 
            : base(path, deletedPath, KeyContainerType.CryptoPro, needDeleted) 
        {
        }
        #endregion

        public override KeyContainerTypeInfoDB Read()
        {
            ReadMainPath();
            if (_needDeleted)
                ReadDeletedPath();
            return _kcti;
        }

        private void ReadMainPath()
        {
            _kcti.Error = GetContainerPaths(_path, out var containerPaths);
            if (_kcti.Error.Length < 1)
                AddRangeKeyContainerInfo(containerPaths);
        }

        private void AddRangeKeyContainerInfo(IEnumerable<string> containerPaths)
        {
            foreach (var containerPath in containerPaths)
                AddKeyContainerInfo(containerPath);
        }

        private void AddKeyContainerInfo(string containerPath)
        {
            var kci = InitWithError(containerPath);
            if (kci.Error.Length < 1)
                kci = ReadKeyContainerInfo(containerPath);
            _kcti.KeyContainersInfoDB.Add(kci);
        }

        private void AddRangeDeletedKeyContainerInfo(IEnumerable<string> containerPaths)
        {
            foreach (var containerPath in containerPaths)
                AddDeletedKeyContainerInfo(containerPath);
        }

        private void AddDeletedKeyContainerInfo(string containerPath)
        {
            var kci = InitWithError(containerPath);
            if (kci.Error.Length < 1)
                kci = ReadKeyContainerInfo(containerPath);
            _kcti.KeyContainersInfoDeletedDB.Add(kci);
        }

        private void ReadDeletedPath()
        {
            if (_kcti.ErrorDeleted.Length > 0)
                return;
            _kcti.ErrorDeleted = GetContainerPaths(_deletedPath, out var deletedСontainerPaths);
            if (_kcti.ErrorDeleted.Length < 1 && deletedСontainerPaths.Any())
                AddRangeDeletedKeyContainerInfo(deletedСontainerPaths);

            AddOldVersionDeletedKeyContainerInfo();
        }

        private KeyContainerInfoDB InitWithError(string path) =>
            new()
            {
                Path = path,
                Error = IsContainerContentValid(path),
            };

        private KeyContainerInfoDB ReadKeyContainerInfo(string path)
        {
            var reader = new CryptoProContainerReader(path);
            return reader.Read();
        }

        private void AddOldVersionDeletedKeyContainerInfo()
        {
            var lastIndex = _deletedPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
            var oldDeletedPath = _deletedPath.Substring(0, lastIndex);
            var oldError = GetContainerPaths(oldDeletedPath, out var oldDeletedСontainerPaths);
            if (oldError.Length < 1 && oldDeletedСontainerPaths.Any())
                AddRangeDeletedKeyContainerInfo(oldDeletedСontainerPaths);
        }
    }
}
