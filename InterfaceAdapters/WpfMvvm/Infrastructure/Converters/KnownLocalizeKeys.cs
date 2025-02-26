namespace WpfMvvm.Infrastructure.Converters
{
    internal static class KnownLocalizeKeys
    {
        internal const string __pluralYearKey = "PluralYear";
        internal const string __pluralMonthKey = "PluralMonth";
        internal const string __pluralDayKey = "PluralDay";
        internal const string __pluralHoursKey = "PluralH";
        internal const string __pluralMinutesKey = "PluralM";
        internal const string __noData = "NoData";
        internal const string __finished = "Finished";

        internal const string __dateAndTimeFormat = "DateAndTimeFormat";
        internal const string __dateAndTimeFormatRu = "dd.MM.yyyy HH:mm:ss";

        internal const string __errorCaption = "ErrorCaption";
        internal const string __btnWebError = "BtnWebError";

        internal const string __savingCertCaption = "SavingCertCaption";
        internal const string __savingCertBody = "SavingCertBody";
        internal const string __savingCertQuestion = "SavingCertQuestion";
        internal const string __buttonYes = "ButtonYes";
        internal const string __buttonCancel = "ButtonCancel";
        internal const string __savingCertsTitleOk = "SavingCertsTitleOk";
        internal const string __savingCertsItemOk = "SavingCertsItemOk";
        internal const string __savingCertsTitleError = "SavingCertsTitleError";
        internal const string __savingCertsItemError = "SavingCertsItemError";

        internal const string __allReaders = "AllReaders";
        internal const string __allKeysMedia = "AllKeysMedia";
        internal const string __flashDrive = "FlashDrive";
        internal const string __localDrive = "LocalDrive";
        internal const string __floppyDrive = "FloppyDrive";
        internal const string __mB = "MB";
        internal const string __gB = "GB";
        internal const string __tB = "TB";

        internal const string __showErrorCaption = "ShowErrorCaption";
        internal const string __showErrorTitleErrors = "ShowErrorTitleErrors";
        internal const string __snapshotTime = "SnapshotTime";

        internal const string __warningCaption = "WarningCaption";
        internal const string __deleteWarningQuestion = "DeleteWarningQuestion";
        internal const string __deletingContainerCaption = "DeletingContainerCaption";
        internal const string __deletingContainerBodyToTrash = "DeletingContainerBody";
        internal const string __deletingContainerBodyPermanently = "DeletingContainerBody2";
        internal const string __deletingContainerBodyError = "DeletingContainerBodyError";
        internal const string __buttonClose = "ButtonClose";
        internal const string __deletingContainersTitleOk = "DeletingContainersTitleOk";
        internal const string __deletingContainersTitleError = "DeletingContainersTitleError";
        internal const string __deletingContainersItemError = "DeletingContainersItemError";

        internal static readonly string[] __knownErrorKeys = 
            [
                __errorKeyKeyContainer,
                __errorKeyContainerType,
                __errorKeyKeyMedia,
                __errorKeyReader,
                __errorKeyClrException
            ];

        internal static readonly string[] __knownErrorDescs = 
            [
                __drivesNotFound,
                __drivesTypeNotFound,
                __drivesReadyNotFound,
                __driveNotReady,
                __systemDrive,
                __keyContainersNotFound,
                __driveNotFound,
                __keyMediaDeleterNotImplemented,
                __keyContainersTypeNotFound,
                __keyDeletedStoreNotFound,
                __keyContainerNotValid,
                __keyFileNotExist,
                __keyFileTooShort,
                __keyFileBadLen,
                __keyContainerCantAssignDeletedName,
                __keyContainerEmptyPath,
                __keyContainerEmptyDeletedPath,
                __keyContainerNotExist, 
                __keyContainerAlreadyExist,
                __keyContainerCertNotFound,
                __keyContainerCertSaveUnknownError,
            ];

        // Error Keys
        private const string __errorKeyKeyContainer = "KeyContainer";
        private const string __errorKeyContainerType = "TypeKeyContainer";
        private const string __errorKeyKeyMedia = "KeyMedia";
        private const string __errorKeyReader = "Reader";
        private const string __errorKeyClrException = "CLRException";
        // Error Descs
        private const string __drivesNotFound = "DrivesNotFound";
        private const string __drivesTypeNotFound = "DrivesTypeNotFound";
        private const string __drivesReadyNotFound = "DrivesReadyNotFound";
        private const string __driveNotReady = "DriveNotReady";
        private const string __systemDrive = "SystemDrive";
        private const string __keyContainersNotFound = "KeyContainersNotFound";
        private const string __driveNotFound = "DriveNotFound";
        private const string __keyMediaDeleterNotImplemented = "KeyMediaDeleterNotImplemented";
        private const string __keyContainersTypeNotFound = "KeyContainersTypeNotFound";
        private const string __keyDeletedStoreNotFound = "KeyDeletedStoreNotFound";
        private const string __keyContainerNotValid = "KeyContainerNotValid";
        private const string __keyFileNotExist = "KeyFileNotExist";
        private const string __keyFileTooShort = "KeyFileTooShort";
        private const string __keyFileBadLen = "KeyFileBadLen";
        private const string __keyContainerCantAssignDeletedName = "KeyContainerCantAssignDeletedName";
        private const string __keyContainerEmptyPath = "KeyContainerEmptyPath";
        private const string __keyContainerEmptyDeletedPath = "KeyContainerEmptyDeletedPath";
        private const string __keyContainerNotExist = "KeyContainerNotExist";
        private const string __keyContainerAlreadyExist = "The key container already exists";
        private const string __keyContainerCertNotFound = "KeyContainerCertNotFound";
        private const string __keyContainerCertSaveUnknownError = "KeyContainerCertSaveUnknownError";
    }
}
