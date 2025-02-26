using System;

namespace PKInfo.Data.DTO
{
    internal class KeyContainerInfoDB
    {
        public string Error { get; set; } = string.Empty;
        public string Name { get; set; }
        public DateTime? DateNotAfterUTC { get; set; }
        public DateTime? DateOfCopyUTC { get; set; }
        public byte[] PublicKey { get; set; } = [];
        public byte[] CertRawData { get; set; } = [];
        public string Path { get; set; }
        public bool IsEncrypted { get; set; }
        public bool? IsExportable { get; set; }
    }
}
