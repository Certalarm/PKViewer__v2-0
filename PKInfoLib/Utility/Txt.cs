using System;
using System.Globalization;

namespace PKInfo.Utility
{
    public static class Txt
    {
        internal static readonly string Rn = Environment.NewLine;
        internal static readonly string FullDateTimePattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        internal const string DateFormatRu = "dd.MM.yyy HH:mm:ss";
        internal const string DateTimeFormatZ = "yyyyMMddHHmmss'Z'";
        internal const char Tab = '\t';
        internal const char Space = ' ';
        internal const char Minus = '-';
        internal const char Equally = '=';
        internal const char Colon = ':';
        internal const char Dot = '.';
        internal const char Comma = ',';
        internal const char LeftSquareBracket = '[';
        internal const char RightSquareBracket = ']';
        internal const char ThreeDot = '…';
        internal const string Empty = "Empty";
        internal const string Pcs = "pcs";

        public const string DELETED_KEY_DIR_NAME = "_oldKey";

        internal const string ErrorBody = nameof(ErrorBody);
        internal const string ErrorDeletedBody = nameof(ErrorDeletedBody);

        internal const string Path = nameof(Path);
        internal const string RootPath = nameof(RootPath);
        internal const string KeyMediaType = nameof(KeyMediaType);
        internal const string TypeKeyContainerType = nameof(TypeKeyContainerType);
        internal const string ReaderType = nameof(ReaderType);

        internal const string SnapshotTime = nameof(SnapshotTime);

        #region Error Keys
        internal const string CLRException = nameof(CLRException);
        private const string Reader = nameof(Reader);
        private const string KeyMedia = nameof(KeyMedia);
        internal const string TypeKeyContainer = nameof(TypeKeyContainer);
        internal const string KeyContainer = nameof(KeyContainer);

        internal static readonly string DrivesNotFound = $"{Reader}{Dot}{nameof(DrivesNotFound)}";
        internal static readonly string DrivesTypeNotFound = $"{Reader}{Dot}{nameof(DrivesTypeNotFound)}";
        internal static readonly string DrivesReadyNotFound = $"{Reader}{Dot}{nameof(DrivesReadyNotFound)}";
        internal static readonly string DriveNotReady = $"{Reader}{Dot}{nameof(DriveNotReady)}";
        internal static readonly string SystemDrive = $"{KeyMedia}{Dot}{nameof(SystemDrive)}";
        internal static readonly string KeyContainersNotFound = $"{KeyMedia}{Dot}{nameof(KeyContainersNotFound)}";
        internal static readonly string DriveNotFound = $"{KeyMedia}{Dot}{nameof(DriveNotFound)}";
        internal static readonly string KeyMediaDeleterNotImplemented = $"{KeyMedia}{Dot}{nameof(KeyMediaDeleterNotImplemented)}";
        internal static readonly string KeyContainersTypeNotFound = $"{TypeKeyContainer}{Dot}{nameof(KeyContainersTypeNotFound)}";
        internal static readonly string KeyDeletedStoreNotFound = $"{TypeKeyContainer}{Dot}{nameof(KeyDeletedStoreNotFound)}";
        internal static readonly string KeyContainerNotValid = $"{KeyContainer}{Dot}{nameof(KeyContainerNotValid)}";
        internal static readonly string KeyFileNotExist = $"{KeyContainer}{Dot}{nameof(KeyFileNotExist)}";
        internal static readonly string KeyFileTooShort = $"{KeyContainer}{Dot}{nameof(KeyFileTooShort)}";
        internal static readonly string KeyFileBadLen = $"{KeyContainer}{Dot}{nameof(KeyFileBadLen)}";
        internal static readonly string KeyContainerCantAssignDeletedName = $"{KeyContainer}{Dot}{nameof(KeyContainerCantAssignDeletedName)}";
        internal static readonly string KeyContainerEmptyPath = $"{KeyContainer}{Dot}{nameof(KeyContainerEmptyPath)}";
        internal static readonly string KeyContainerEmptyDeletedPath = $"{KeyContainer}{Dot}{nameof(KeyContainerEmptyDeletedPath)}";
        internal static readonly string KeyContainerNotExist = $"{KeyContainer}{Dot}{nameof(KeyContainerNotExist)}";
        internal static readonly string KeyContainerAlreadyExist = $"{KeyContainer}{Dot}{nameof(KeyContainerAlreadyExist)}";
        internal static readonly string KeyContainerCertNotFound = $"{KeyContainer}{Dot}{nameof(KeyContainerCertNotFound)}";
        internal static readonly string KeyContainerCertSaveUnknownError = $"{KeyContainer}{Dot}{nameof(KeyContainerCertSaveUnknownError)}";
        #endregion
    }
}
