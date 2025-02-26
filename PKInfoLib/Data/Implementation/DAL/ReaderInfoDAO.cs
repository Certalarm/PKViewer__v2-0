using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Data.Implementation.DAO
{
    internal sealed class ReaderInfoDAO
    {
        private readonly IDatabase _Database; 

        #region .ctors
        internal ReaderInfoDAO(IDatabase db)
        {
            _Database = db;
        }
        #endregion

        internal string DeleteKeyContainer(string path) => _Database.DeleteKeyContainer(path);

        internal IList<ReaderInfo> GetReadersInfo(bool needDeleted) =>
            _Database.LoadReadersInfo(needDeleted)
                .Select(x => x.AsReaderInfo(needDeleted))
                .ToList();
    }
}
