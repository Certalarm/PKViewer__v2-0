using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DAL
{
    internal static class RepositoryHelper
    {
        internal static IEnumerable<KeyContainerInfo> GetKeyContainersAll(IList<ReaderInfo> readers) =>
            GetAllKeyMedias(readers)
                ?.SelectMany(x => GetKeyContainersByMediaPath(x.RootPath, readers))
                ?? [];

        internal static IEnumerable<KeyContainerInfo> GetKeyContainersByMediaPath(string mediaPath, IList<ReaderInfo> readers) =>
            GetKeyMedia(mediaPath, readers)
                ?.KeyContainersInfo
                ?? [];

        internal static IEnumerable<KeyContainerInfo> GetKeyContainersByPaths(string[] containerPaths, IList<ReaderInfo> readers)
        {
            foreach (string path in containerPaths) 
                yield return GetKeyContainerByPath(path, readers);
        }

        internal static KeyContainerInfo GetKeyContainerByPath(string containerPath, IList<ReaderInfo> readers)
        {
            if (string.IsNullOrWhiteSpace(containerPath))
                return InitContainerInfoErrorOnly(KeyContainerEmptyPath);
            var containerInfo = GetKeyContainer(containerPath, readers);
            return containerInfo is null
                ? InitContainerInfoErrorOnly(KeyContainerNotExist)
                : containerInfo;
        }

        internal static IEnumerable<KeyMediaInfo> GetKeyMediasByType(KeyMediaType keyMediaType, IList<ReaderInfo> readers)
        {
            var keyMedias = GetKeyMedias(keyMediaType, readers);
            return keyMedias.Any()
                ? keyMedias
                : [];
        }

        internal static KeyMediaInfo GetKeyMediaByPath(string mediaPath, IList<ReaderInfo> readers)
        {
            var keyMedia = GetKeyMedia(mediaPath, readers);
            return keyMedia is null
                ? InitKeyMediaInfoErrorOnly(DriveNotFound)
                : keyMedia;
        }

        internal static ReaderInfo GetReaderByType(ReaderType readerType, IList<ReaderInfo> readers)
        {
            var reader = GetReader(readerType,readers);
            return reader is null
                ? InitReaderInfoErrorOnly(DrivesTypeNotFound)
                : reader;
        }

        private static KeyContainerInfo GetKeyContainer(string path, IList<ReaderInfo> readers) =>
            GetAllKeyMedias(readers)
                ?.SelectMany(x => x.KeyContainersInfo)
                .FirstOrDefault(x => x.Path.EqualsIgnoreCase(path));

        private static IEnumerable<KeyMediaInfo> GetKeyMedias(KeyMediaType keyMediaType, IList<ReaderInfo> readers) =>
            GetAllKeyMedias(readers)
                ?.Where(x => x.Type == keyMediaType)
                ?? [];

        private static KeyMediaInfo GetKeyMedia(string path, IList<ReaderInfo> readers) =>
            GetAllKeyMedias(readers)
                ?.FirstOrDefault(x => x.RootPath.EqualsIgnoreCase(path));

        private static IEnumerable<KeyMediaInfo> GetAllKeyMedias(IList<ReaderInfo> readers) =>
            readers
                ?.SelectMany(x => x.KeyMediasInfo)
                ?? [];

        private static ReaderInfo GetReader(ReaderType readerType, IList<ReaderInfo> readers) =>
            readers
                .FirstOrDefault(x => x.Type == readerType);

        private static KeyContainerInfo InitContainerInfoErrorOnly(string error) =>
            new()
            {
                Error = error,
            };

        private static KeyMediaInfo InitKeyMediaInfoErrorOnly(string error) =>
            new()
            {
                Error = error,
            };

        private static ReaderInfo InitReaderInfoErrorOnly(string error) =>
            new()
            {
                Error = error,
            };
    }
}
