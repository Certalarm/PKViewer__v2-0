using PKInfo.Utility.Enum;
using System;
using static PKInfo.Utility.Exts;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    internal sealed class KeyContainerInfo()
    {
        public string Name { get; set; }
        public KeyContainerType Type { get; set; }
        public string Error { get; set; } = string.Empty;
        public DateTime? DateNotAfterUTC { get; set; }
        public DateTime? DateOfCopyUTC { get; set; }
        public byte[] PublicKey { get; set; } = [];
        public byte[] CertRawData { get; set; } = [];
        public string Path { get; set; }
        public bool IsEncrypted { get; set; }
        public bool? IsExportable { get; set; }
        public bool? IsDeleted { get; set; }

#if DEBUG
        public override string ToString() =>
            LeftSquareBracket
            + BuildLine(nameof(Name), Name)
            + BuildLine(nameof(Type), Type.ToString())
            + BuildLine(nameof(Error), Error)
            + BuildLine(nameof(DateNotAfterUTC), DateToString(DateNotAfterUTC))
            + BuildLine(nameof(DateOfCopyUTC), DateToString(DateOfCopyUTC))
            + BuildLine(nameof(PublicKey), BytesToString(PublicKey))
            + BuildLine(nameof(CertRawData), BytesToString(CertRawData))
            + BuildLine(nameof(Path), Path)
            + BuildLine(nameof(IsEncrypted), IsEncrypted.ToString())
            + BuildLine(nameof(IsExportable), IsExportable.ToString())
            + BuildLine(nameof(IsDeleted), IsDeleted?.ToString() ?? "Not Supported")
            + Rn + RightSquareBracket;

        private static string DateToString(DateTime? dateTime) =>
            dateTime?.ToString(DateFormatRu) ?? string.Empty;

        private static string BuildLine(string key, string value) =>
            $"{Rn}{Tab}{key}{Colon}{Space}{value}{Comma}";
#endif
    }
}
