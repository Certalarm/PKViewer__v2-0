using PKInfo.Data.DTO;
using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Data.Implementation.DAL.DeleterDetector;

namespace PKInfo.Data.Implementation.DAL
{
    internal static class ReaderInfoDAOExts
    {
        internal static ReaderInfo AsReaderInfo(this ReaderInfoDB riDB, bool needDeleted) =>
            new()
            {
                Type = riDB.Type,
                Error = riDB.Error,
                KeyMediasInfo = riDB.KeyMeadiasInfoDB.AsKeyMediasInfo(needDeleted),
            };

        private static IList<KeyMediaInfo> AsKeyMediasInfo(this IList<KeyMediaInfoDB> kmiDBs, bool needDeleted)
        {   
            IList<KeyMediaInfo> result = [];
            foreach (var kmiDB in kmiDBs)
            {
                KeyMediaInfo kmi = InitByKeyMediaInfoDB(kmiDB);
                kmi.KeyContainersInfo = kmiDB.KeyContainerTypesInfoDB.AsInfo(kmi.Type, needDeleted);
                result.Add(kmi);
            }
            return result;
        }

        private static IList<KeyContainerInfo> AsInfo(this IList<KeyContainerTypeInfoDB> kctiDBs, KeyMediaType kmType, bool needDeleted)
        {
            List<KeyContainerInfo> result = [];
            foreach (var kctiDB in kctiDBs)
                result.AddRange([.. GetMain(kctiDB, kmType), .. GetDeleted(kctiDB, needDeleted)]);
            return result;
        }

        private static IList<KeyContainerInfo> GetMain(KeyContainerTypeInfoDB kctiDB, KeyMediaType kmType) =>
            IsNotNeedMain(kctiDB)
                ? []
                : kctiDB.KeyContainersInfoDB.AsInfo(kctiDB.Type, GetIsDeletedForMain(kmType, kctiDB.Type));

        private static bool? GetIsDeletedForMain(KeyMediaType kmType, KeyContainerType kcType) =>
            GetDeleter((kmType, kcType)) is null
                ? null
                : false;

        private static IList<KeyContainerInfo> GetDeleted(KeyContainerTypeInfoDB kctiDB, bool needDeleted) =>
            IsNotNeedDeleted(needDeleted, kctiDB)
                ? []
                : kctiDB.KeyContainersInfoDeletedDB.AsInfo(kctiDB.Type, true);

        private static bool IsNotNeedMain(KeyContainerTypeInfoDB kctiDB) =>
            kctiDB.Error.Length > 0 || !kctiDB.KeyContainersInfoDB.Any();

        private static bool IsNotNeedDeleted(bool needDeleted, KeyContainerTypeInfoDB kctiDB) =>
            !needDeleted || kctiDB.ErrorDeleted.Length > 0 || !kctiDB.KeyContainersInfoDeletedDB.Any();

        private static IList<KeyContainerInfo> AsInfo(this IList<KeyContainerInfoDB> kciDBs, KeyContainerType kcType, bool? isDeleted)
            => kciDBs
                .Select(x => BuildKeyContainerInfo(x, kcType, isDeleted))
                .ToList();

        private static KeyContainerInfo BuildKeyContainerInfo(KeyContainerInfoDB kciDB, KeyContainerType kcType, bool? isDeleted)
        {
            var result = InitByKeyContainerInfoDB(kciDB);
            result.Type = kcType;
            result.IsDeleted = isDeleted;
            return result;
        }

        private static KeyMediaInfo InitByKeyMediaInfoDB(KeyMediaInfoDB kmiDB) =>
            new()
            {
                RootPath = kmiDB.RootPath,
                Label = kmiDB.Label,
                Size = kmiDB.Size,
                Type = kmiDB.Type,
                Error = kmiDB.Error,
                ErrorsKeyContainerTypes = kmiDB.KeyContainerTypesInfoDB
                    .Select(x => new ErrorKeyContainerTypeInfo(x.Type, x.Error, x.ErrorDeleted))
                    .ToList()
            };

        private static KeyContainerInfo InitByKeyContainerInfoDB(KeyContainerInfoDB kciDB) =>
            new ()
            {
                Name = kciDB.Name,
                Error = kciDB.Error,
                DateNotAfterUTC = kciDB.DateNotAfterUTC,
                DateOfCopyUTC = kciDB.DateOfCopyUTC,
                PublicKey = kciDB.PublicKey,
                CertRawData = kciDB.CertRawData,
                Path = kciDB.Path,
                IsEncrypted = kciDB.IsEncrypted,
                IsExportable = kciDB.IsExportable,
            };
    }
}
