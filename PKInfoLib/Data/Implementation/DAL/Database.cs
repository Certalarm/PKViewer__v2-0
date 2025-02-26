using PKInfo.Data.DTO;
using PKInfo.Data.Implementation.DiskReader;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Data.Implementation.DAL.DeleterDetector;

namespace PKInfo.Data.Implementation.DAL
{
    internal class Database : IDatabase
    {
        public string DeleteKeyContainer(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Txt.KeyContainerEmptyPath;
            var readers = LoadReadersInfo(needDeleted: true);
            return readers.Any()
                ? Delete(path, readers)
                : Txt.DrivesNotFound;
        }

        public IList<ReaderInfoDB> LoadReadersInfo(bool needDeleted)
        {
            IList<ReaderInfoDB> result = [];
            foreach (var type in ImplementedReaderTypes())
            {
                var reader = GetReader(type, needDeleted);
                result.Add(reader.Read());
            };
            return result;
        }

        private string Delete(string path, IList<ReaderInfoDB> readers)
        {
            var deleter = GetDeleter(DetectTypes(path, readers));
            return deleter is null
                ? Txt.KeyMediaDeleterNotImplemented
                : deleter.Delete(path);
        }

        private IInfoReaderDB<ReaderInfoDB> GetReader(ReaderType readerType, bool needDeleted) =>
            readerType switch
            {
                ReaderType.AllRemovable => new RemovableDiskReader(needDeleted),
                ReaderType.AllFixed => new FixedDiskReader(needDeleted),
                _ => throw new NotImplementedException(readerType.ToString())
            };

        private static (KeyMediaType, KeyContainerType) DetectTypes(string path, IList<ReaderInfoDB> readers)
        {
            (KeyMediaType, KeyContainerType) result = (KeyMediaType.NotImplemented, KeyContainerType.NotImplemented);
            foreach (var keyMedia in GetKeyMediasInfoDB(readers))
                foreach (var keyContainerType in keyMedia.KeyContainerTypesInfoDB)
                {
                    var keyContainer = GetKeyContainerByPath(path, keyContainerType);
                    if (keyContainer is not null)
                        return (keyMedia.Type, keyContainerType.Type);
                }
            return result;
        }

        private static KeyContainerInfoDB GetKeyContainerByPath(string path, KeyContainerTypeInfoDB containersType) =>
            containersType.KeyContainersInfoDB
                .Concat(containersType.KeyContainersInfoDeletedDB)
                .Where(x => x.Path.EqualsIgnoreCase(path))
                .FirstOrDefault();

        private static IEnumerable<KeyMediaInfoDB> GetKeyMediasInfoDB(IList<ReaderInfoDB> readers)
        {
            foreach (var reader in readers)
                foreach (var keyMedia in reader.KeyMeadiasInfoDB)
                        yield return keyMedia;
        }

        private static IEnumerable<ReaderType> ImplementedReaderTypes() =>
            Enum.GetValues(typeof(ReaderType))
                .OfType<ReaderType>()
                .Where(x => x != ReaderType.NotImplemented);
    }
}
