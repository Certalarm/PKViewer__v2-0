using PKInfo.Data.DTO;
using PKInfo.Data.Interface;
using System.Text;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal abstract class BaseDiskKeyContainerReader : IInfoReaderDB<KeyContainerInfoDB>
    {
        protected KeyContainerInfoDB _kci;
        protected Encoding _codePage;

        #region .ctors
        protected BaseDiskKeyContainerReader(string path, int codePage)
        {
            _codePage = Encoding.GetEncoding(codePage);
            _kci = new KeyContainerInfoDB
            {
                Path = path,
            };
        }
        #endregion

        public virtual KeyContainerInfoDB Read()
        {
            return _kci;
        }
    }
}
