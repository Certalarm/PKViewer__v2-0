using PKInfo.Data.DTO;
using System.Collections.Generic;

namespace PKInfo.Data.Interface
{
    internal interface IDatabase
    {
        public IList<ReaderInfoDB> LoadReadersInfo(bool needDeleted);

        public string DeleteKeyContainer(string path);
    }
}
