using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Domain.Entity
{
    public sealed class KeyMediaSnapshotInfo
    {
        public string RootPath { get; private set; }
        public string Label { get; private set; }
        public long Size { get; private set; }
        public KeyMediaType Type { get; private set; }
        public IList<KeyContainerSnapshotInfo> KeyContainersInfo { get; private set; } = [];


        #region .ctors
        private KeyMediaSnapshotInfo()
        {                       
        }
        #endregion

        internal static KeyMediaSnapshotInfo Create(KeyMediaInfo keyMediaInfo, DateTime snapshotTime) =>
            keyMediaInfo.Error.Length > 0
                ? null
                : Init(keyMediaInfo, snapshotTime);

        private static KeyMediaSnapshotInfo Init(KeyMediaInfo keyMediaInfo, DateTime snapshotTime) =>
            new()
            {
                Type = keyMediaInfo.Type,
                RootPath = keyMediaInfo.RootPath,
                Label = keyMediaInfo.Label,
                Size = keyMediaInfo.Size,
                KeyContainersInfo = keyMediaInfo.KeyContainersInfo
                    .Where(x => x.Error.Length < 1)
                    .Select(x => KeyContainerSnapshotInfo.Create(x, snapshotTime))
                    .ToList(),
            };
    }
}
