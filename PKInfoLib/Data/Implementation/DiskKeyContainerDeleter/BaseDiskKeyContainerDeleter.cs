using PKInfo.Data.Interface;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerDeleter
{
    internal abstract class BaseDiskKeyContainerDeleter : IDeleter
    {

        #region .ctors
        protected BaseDiskKeyContainerDeleter()
        {
        }
        #endregion

        public virtual string Delete(string path) =>
            string.IsNullOrWhiteSpace(path)
                ? KeyContainerEmptyPath
                : DeleteIfNotEmptyPath(path);

        protected abstract string DeleteIfNotEmptyPath(string path);
    }
}
