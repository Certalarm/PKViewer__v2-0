using PKInfo.Domain.Entity.Base;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public class RootError : BaseError
    {
        public IReadOnlyList<ReaderError> ReaderErrors { get; } = [];

        #region .ctors
        private RootError(DateTime snapshotTime, List<ReaderError> readerErrors) 
            : base((int)DepthLevelType.RootDepth, string.Empty)
        {
            SetIfNotEmpty(SnapshotTime, snapshotTime.ToString(FullDateTimePattern));
            ReaderErrors = readerErrors;
        }

        private RootError(Dictionary<string, string> content, List<ReaderError> readerErrors)
            : base((int)DepthLevelType.RootDepth, string.Empty)
        {
            Content = content;
            ReaderErrors = readerErrors;
        }
        #endregion

        public static RootError Create(Dictionary<string, string> content, List<ReaderError> readerErrors)
        {
            return new(content, readerErrors);
        }

        internal static RootError Create(DateTime snapshotTime, List<ReaderInfo> readerInfos)
        {
            var readerErrors = readerInfos
                .Select(ReaderError.Create)
                .Where(x => x.Content.HasKey(ErrorBody) || x.KeyMediaErrors.Count > 0)
                .ToList();
            return new(snapshotTime, readerErrors);
        }

#if DEBUG
        public override string ToString()
        {
            return base.ToString()
                + $"{Rn}{Tab}{nameof(ReaderErrors)}{Colon}{Rn}"
                + string.Join(Rn + Tab, ReaderErrors.Select(x => x.ToString()))
                ;
        }
#endif
    }
}
