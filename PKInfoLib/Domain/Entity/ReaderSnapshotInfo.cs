using System;
using System.Collections.Generic;
using System.Linq;
using PKInfo.Utility.Enum;

namespace PKInfo.Domain.Entity
{
    public class ReaderSnapshotInfo
    {
        public ReaderType Type { get; private set; }
        public IList<KeyMediaSnapshotInfo> KeyMediasInfo { get; private set; } = [];

        #region .ctors
        private ReaderSnapshotInfo()
        {                      
        }
        #endregion

        internal static ReaderSnapshotInfo Create(ReaderInfo readerInfo, DateTime snapshotTime) =>
            Init(readerInfo, snapshotTime);

        private static ReaderSnapshotInfo Init(ReaderInfo readerInfo, DateTime snapshotTime) =>
            new()
            {
                Type = readerInfo.Type,
                KeyMediasInfo = readerInfo.KeyMediasInfo
                    .Where(x => x.Error.Length < 1)
                    .Select(x => KeyMediaSnapshotInfo.Create(x, snapshotTime))
                    .ToList(),
            };
    }
}
