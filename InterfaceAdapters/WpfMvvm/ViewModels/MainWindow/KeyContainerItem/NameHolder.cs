using PKInfo.Domain.Entity;

namespace WpfMvvm.Models.KeyContainerItem
{
    public class NameHolder
    {
        public string Name { get; private set; }
        public bool? IsEncrypted { get; private set; }
        public bool? IsExportable { get; private set; }
        public bool? IsDeleted { get; private set; }
        public bool? IsCertPresent { get; private set; }

        #region .ctors
        private NameHolder()
        {
        }
        #endregion

        public static NameHolder Create(KeyContainerSnapshotInfo keyInfo) =>
            new()
            {
                Name = keyInfo.Name,
                IsEncrypted = keyInfo.IsEncrypted,
                IsExportable = keyInfo.IsExportable,
                IsDeleted = keyInfo.IsDeleted,
                IsCertPresent = keyInfo.IsCertPresent
            };

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NameHolder);
        }

        public bool Equals(NameHolder other) =>
            other is not null
                && string.Equals(Name, other.Name, System.StringComparison.OrdinalIgnoreCase)
                && IsEncrypted == other.IsEncrypted
                && IsExportable == other.IsExportable
                && IsDeleted == other.IsDeleted
                && IsCertPresent == other.IsCertPresent;

        public override int GetHashCode() => base.GetHashCode();
    }
}
