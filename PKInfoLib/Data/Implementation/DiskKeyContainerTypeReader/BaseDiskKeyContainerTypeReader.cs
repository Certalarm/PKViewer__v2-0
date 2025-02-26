using PKInfo.Data.DTO;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerTypeReader
{
    internal abstract class BaseDiskKeyContainerTypeReader : IInfoReaderDB<KeyContainerTypeInfoDB>
    {
        protected KeyContainerTypeInfoDB _kcti;
        protected string _path;
        protected string _deletedPath;
        protected bool _needDeleted;

        #region .ctors
        public BaseDiskKeyContainerTypeReader(string path, string deletedPath, KeyContainerType type, bool needDeleted)
        {
            _path = path;
            _deletedPath = deletedPath;
            _needDeleted = needDeleted;
            _kcti = new()
            {
                Type = type,
            };
            InitErrorDeleted();
        }
        #endregion

        private void InitErrorDeleted()
        {
            if (string.IsNullOrEmpty(_deletedPath))
                _kcti.ErrorDeleted = KeyDeletedStoreNotFound;
        }

        public abstract KeyContainerTypeInfoDB Read();
    }
}
