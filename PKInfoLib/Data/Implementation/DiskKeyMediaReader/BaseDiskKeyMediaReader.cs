using PKInfo.Data.DTO;
using PKInfo.Data.Implementation.DiskKeyContainerDeleter;
using PKInfo.Data.Implementation.DiskKeyContainerTypeReader;
using PKInfo.Data.Interface;
using PKInfo.Utility;
using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PKInfo.Data.Implementation.DiskKeyMediaReader
{
    internal abstract class BaseDiskKeyMediaReader: IInfoReaderDB<KeyMediaInfoDB>
    {
        protected DriveMetaInfo _metaInfo;
        protected KeyMediaType _type;
        protected bool _needDeleted;
        protected string _error = string.Empty;
        protected string _errorDeleted = string.Empty;

        #region .ctors
        protected BaseDiskKeyMediaReader(DriveMetaInfo metaInfo, KeyMediaType type, bool needDeleted)
        {
            _metaInfo = metaInfo;
            _type = type;
            _needDeleted = needDeleted;
        }
        #endregion

         public virtual KeyMediaInfoDB Read()
        {
            var kmi = InitKeyMediaInfo();
            foreach(var containerType in ImplementedKeyContainerTypes())
            {
                var kcti = ReadKeyContainerTypeInfo(containerType);
                kmi.KeyContainerTypesInfoDB.Add(kcti);
            } 
            if (IsNotKeyContainersAnyTypeFound(kmi))
                kmi.Error = Txt.KeyContainersNotFound;
            return kmi;
        }

        private bool IsNotKeyContainersAnyTypeFound(KeyMediaInfoDB kmi) =>
            !kmi.KeyContainerTypesInfoDB
                .Any(x => x.Error.Length < 1 || x.ErrorDeleted.Length < 1);

        private KeyContainerTypeInfoDB ReadKeyContainerTypeInfo(KeyContainerType containerType)
        {
            var deletedPath = GetDeletedKeyStorePath(containerType);
            return GetReader(containerType, deletedPath).Read();
        }

        private KeyMediaInfoDB InitKeyMediaInfo() =>
            new()
            {
                RootPath = _metaInfo.Name,
                Label = _metaInfo.Label,
                Size = _metaInfo.Size,
                Type = _type,
            };

        private IInfoReaderDB<KeyContainerTypeInfoDB> GetReader(KeyContainerType containerType, string deletedPath) 
            => containerType switch
            {
                KeyContainerType.CryptoPro => new CryptoProKeyContainerTypeReader(_metaInfo.Name, deletedPath, _needDeleted),
                _ => throw new NotImplementedException(containerType.ToString()),
            };

        private string GetDeletedKeyStorePath(KeyContainerType keyType)
        {
            var storePath = DirectoryNamer.BuildDeletedKeyStoreFullPaths(_metaInfo.Name, keyType);
            return Directory.Exists(storePath)
                ? storePath
                : string.Empty;
        }

        private static IEnumerable<KeyContainerType> ImplementedKeyContainerTypes() =>
            System.Enum.GetValues(typeof(KeyContainerType))
                .OfType<KeyContainerType>()
                .Where(x => x != KeyContainerType.NotImplemented);

    }
}
