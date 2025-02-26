using PKInfo.Utility.Enum;
using System;
using static PKInfo.Utility.Exts;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public sealed class KeyContainerSnapshotInfo
    {
        public KeyContainerType Type { get; private set; } = KeyContainerType.NotImplemented;
        public string Name { get; private set; }
        public DateTime? DateNotAfterUTC { get; private set; }
        public DateTime? DateOfCopyUTC { get; private set; }
        public TimeInterval RemainingTimeUntilEndKey { get; private set; }
        public TimeInterval ElapsedTimeAfterCopy { get; private set; }
        public string PublicKey { get; private set; }
        public string Path { get; private set; }
        public bool IsEncrypted { get; private set; }
        public bool? IsExportable { get; private set; }
        public bool? IsDeleted { get; private set; }
        public bool IsCertPresent { get; private set; }

        #region .ctors
        private KeyContainerSnapshotInfo() { }
        #endregion

        internal static KeyContainerSnapshotInfo Create(KeyContainerInfo containerInfo, DateTime snapshotTime) =>
            containerInfo.Error.Length > 0
            ? null
            : InitWoError(containerInfo, snapshotTime);

        private static KeyContainerSnapshotInfo InitWoError(KeyContainerInfo containerInfo, DateTime snapshotTime) =>
            new()
            {
                Type = containerInfo.Type,
                Name = containerInfo.Name,
                DateNotAfterUTC = containerInfo.DateNotAfterUTC,
                DateOfCopyUTC = containerInfo.DateOfCopyUTC,
                RemainingTimeUntilEndKey = new TimeInterval(snapshotTime.ToUniversalTime(), containerInfo.DateNotAfterUTC),
                ElapsedTimeAfterCopy = new TimeInterval(containerInfo.DateOfCopyUTC, snapshotTime.ToUniversalTime()),
                PublicKey = BytesToString(containerInfo.PublicKey),
                Path = containerInfo.Path,
                IsEncrypted = containerInfo.IsEncrypted,
                IsExportable = containerInfo.IsExportable,
                IsDeleted = containerInfo.IsDeleted,
                IsCertPresent = containerInfo.CertRawData.Length > 0,
            };

#if DEBUG
        public override string ToString() =>
             LeftSquareBracket
             + BuildLine(nameof(Type), Type.ToString())
             + BuildLine(nameof(Name), Name)
             + BuildLine(nameof(DateNotAfterUTC), DateToString(DateNotAfterUTC))
             + BuildLine(nameof(DateOfCopyUTC), DateToString(DateOfCopyUTC))
             + BuildLine(nameof(RemainingTimeUntilEndKey), TimeIntervalToString(RemainingTimeUntilEndKey))
             + BuildLine(nameof(ElapsedTimeAfterCopy), TimeIntervalToString(ElapsedTimeAfterCopy))
             + BuildLine(nameof(PublicKey), PublicKey)
             + BuildLine(nameof(Path), Path)
             + BuildLine(nameof(IsEncrypted), IsEncrypted.ToString())
             + BuildLine(nameof(IsExportable), IsExportable.ToString())
             + BuildLine(nameof(IsDeleted), IsDeleted?.ToString() ?? "Not Supported")
             + BuildLine(nameof(IsCertPresent), IsCertPresent.ToString())
             + Rn + RightSquareBracket;

        private static string DateToString(DateTime? dateTime) =>
            dateTime?.ToString(DateFormatRu) ?? string.Empty;

        private static string TimeIntervalToString(TimeInterval ti) =>
            ti?.IsEmpty ?? true
                ? "No Data"
                : $"{ti.Years}{Space}years{Comma}{Space}"
                    + $"{ti.Months}{Space}months{Comma}{Space}"
                    + $"{ti.Days}{Space}days{Comma}{Space}"
                    + $"{ti.Hours}{Space}hours{Comma}{Space}"
                    + $"{ti.Minutes}{Space}minutes";

        private static string BuildLine(string key, string value) =>
            $"{Rn}{Tab}{key}{Colon}{Space}{value}{Comma}";
#endif
    }
}
