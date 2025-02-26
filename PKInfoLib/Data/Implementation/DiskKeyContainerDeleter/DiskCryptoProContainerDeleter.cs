using static PKInfo.Data.Implementation.DiskKeyContainerDeleter.CryptoProDirectoryNamer;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerDeleter
{
    internal class DiskCryptoProContainerDeleter : BaseDiskKeyContainerDeleter
    {
        #region .ctors
        public DiskCryptoProContainerDeleter(): base()
        {
        }
        #endregion

        protected override string DeleteIfNotEmptyPath(string path) =>
            DirectoryNamer.IsPathContainsDeletedKeyStore(path)
                ? DeleteCompletely(path)
                : MoveToDeletedKeyStore(path);

        private string MoveToDeletedKeyStore(string path)
        {
            var deletedDirFullPath = GetCPDeletedContainerFullPath(path);
            return deletedDirFullPath.Length > 0
                ? DirectoryEditor.MoveDirectory(path, deletedDirFullPath)
                : KeyContainerCantAssignDeletedName;
        }

        private string DeleteCompletely(string path) => DirectoryEditor.DeleteDirectory(path);
    }
}
