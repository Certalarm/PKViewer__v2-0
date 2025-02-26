using PKInfo.Utility.Enum;

namespace PKInfo.Domain.Entity
{
    public sealed class ErrorKeyContainerTypeInfo
    {
        internal KeyContainerType KeyContainerType { get; set; } = KeyContainerType.NotImplemented;
        internal string Error { get; set; } = string.Empty;
        internal string ErrorDeleted { get; set; } = string.Empty;

        #region .ctors
        public ErrorKeyContainerTypeInfo(KeyContainerType type, string error, string errorDeleted)
        {
            KeyContainerType = type;
            Error = error;
            ErrorDeleted = errorDeleted;
        }
        #endregion
    }
}
