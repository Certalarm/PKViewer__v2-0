using System;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public sealed class SelfInfo
    {
        public string Name { get; }
        public string Version { get; }
        public DateTime ReleaseDate { get; }

        #region .ctors
        public SelfInfo(string name, string version, DateTime releaseDate)
        {
            Name = name;
            Version = version;
            ReleaseDate = releaseDate;
        }
        #endregion

#if DEBUG
        public override string ToString() =>
            LeftSquareBracket
            + Rn + Tab + $"{nameof(Name)}{Colon}{Space}{Name},"
            + Rn + Tab + $"{nameof(Version)}{Colon}{Space}{Version},"
            + Rn + Tab + $"{nameof(ReleaseDate)}{Colon}{Space}{ReleaseDate.Date.ToString(DateFormatRu)}"
            + Rn + RightSquareBracket;
#endif
    }
}
