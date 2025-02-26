using PKInfo.Data.DTO;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using PKInfoLib.Tests.DAL.Deleter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static PKInfoLib.Tests.DAL.Generator.CertRawDataStore;
using static PKInfoLib.Tests.DAL.Generator.CPKeyContainerInfoDBHelper;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.DAL
{
    internal class TestDatabase : IDatabase
    {
        static double _utcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalHours;

        private IList<ReaderInfoDB> _readers = [];

        #region .ctors
        public TestDatabase()
        {
        }
        #endregion

        public string DeleteKeyContainer(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Txt.KeyContainerEmptyPath;
            if(!_readers.Any())
                _readers = LoadReadersInfo(needDeleted: true);
            return _readers.Any()
                ? Delete(path)
                : Txt.DrivesNotFound;
        }

        public IList<ReaderInfoDB> LoadReadersInfo(bool needDeleted)
        {
            IList<(string, bool)> allFixedKmiParams =
                [ // format: label, needDeleted
                    ("TEST LOCAL DRIVE #1", false),
                    ("TEST SYSTEM LOCAL DRIVE", false),
                ];

            ReaderInfoDB reader1 = CreateAllFixedReaderInfoDB(allFixedKmiParams);

            IList<(string, bool)> allRemovableKmiParams =
                [ // format: label, needDeleted
                    ("TEST FLOPPY DRIVE #1", true),
                    ("TEST FLASH DRIVE #1", false),
                ];

            ReaderInfoDB reader2 = CreateAllRemovableReaderInfoDB(allRemovableKmiParams);

            ReaderInfoDB reader3 = CreateNotImplementedReaderInfoDB();

            return [ reader1, reader2, reader3 ];
        }

        private static ReaderInfoDB CreateAllFixedReaderInfoDB(IList<(string, bool)> kmiParams) =>
            new()
            {
                Type = ReaderType.AllFixed,
                KeyMeadiasInfoDB = CreateFixedKeyMeadiaInfosDB(kmiParams),
            };

        private static ReaderInfoDB CreateAllRemovableReaderInfoDB(IList<(string, bool)> kmiParams) =>
            new()
            {
                Type = ReaderType.AllRemovable,
                KeyMeadiasInfoDB = CreateRemovableKeyMeadiaInfosDB(kmiParams),
            };

        private static ReaderInfoDB CreateNotImplementedReaderInfoDB() =>
            new()
            {
                Type = ReaderType.NotImplemented,
                KeyMeadiasInfoDB = [CreateNotImplementedKeyMediaDriveX()],
            };

        private static IList<KeyMediaInfoDB> CreateFixedKeyMeadiaInfosDB(IList<(string label, bool needDeleted)> kmiParams) =>
            [
                CreateKeyMediaLocalDriveD(kmiParams[0].label, kmiParams[0].needDeleted),
                CreateKeyMediaSystemLocalDriveC(kmiParams[1].label, kmiParams[1].needDeleted),
            ];

        private static IList<KeyMediaInfoDB> CreateRemovableKeyMeadiaInfosDB(IList<(string label, bool needDeleted)> kmiParams) =>
            [
                CreateKeyMediaFloppyDriveA(kmiParams[0].label, kmiParams[0].needDeleted),
                CreateKeyMediaFlashDriveE(kmiParams[1].label, kmiParams[1].needDeleted),
            ];

        private static KeyMediaInfoDB CreateKeyMediaSystemLocalDriveC(string label, bool needDeleted)
        {
            var rootPath = "C:\\";
            return new()
            {
                Error = Txt.SystemDrive,
                Type = KeyMediaType.LocalDrive,
                Size = new Random().Next(1_024, 1_000_000),
                RootPath = rootPath,
                Label = label,
            };
        }

        private static KeyMediaInfoDB CreateKeyMediaLocalDriveD(string label, bool needDeleted)
        {
            var rootPath = "D:\\";
            var containers = GenContainersDiskD();
            return new()
            {
                Type = KeyMediaType.LocalDrive,
                Size = new Random().Next(1_024, 1_000_000),
                RootPath = rootPath,
                Label = label,
                KeyContainerTypesInfoDB = CreateKeyContainerTypesInfosDB(rootPath, needDeleted, containers),
            };
        }

        private static KeyMediaInfoDB CreateNotImplementedKeyMediaDriveX()
        {
            var rootPath = "X:\\";
            return new()
            {
                Error = Txt.DriveNotFound,
                Type = KeyMediaType.NotImplemented,
                Size = new Random().Next(1_024, 1_000_000),
                RootPath = rootPath,
            };
        }

        private static KeyMediaInfoDB CreateKeyMediaFloppyDriveA(string label, bool needDeleted)
        {
            var rootPath = "A:\\";
            var containers = GenContainersDiskA();
            return new()
            {
                Type = KeyMediaType.FloppyDrive,
                Size = new Random().Next(1_024, 1_000_000),
                RootPath = rootPath,
                Label = label,
                KeyContainerTypesInfoDB = CreateKeyContainerTypesInfosDB(rootPath, needDeleted, containers),
            };
        }

        private static KeyMediaInfoDB CreateKeyMediaFlashDriveE(string label, bool needDeleted)
        {
            var rootPath = "E:\\";
            var containers = GenContainersDiskE();
            return new()
            {
                Type = KeyMediaType.FlashDrive,
                Size = new Random().Next(1_024, 1_000_000),
                RootPath = rootPath,
                Label = label,
                KeyContainerTypesInfoDB = CreateKeyContainerTypesInfosDB(rootPath, needDeleted, containers),
            };
        }

        private static IList<KeyContainerTypeInfoDB> CreateKeyContainerTypesInfosDB(string rootPath, bool needDeleted, IList<KeyContainerInfoDB> containers) =>
            [
                CreateCryptoProKeyContainerTypeInfoDB(rootPath, needDeleted, containers),
            ];

        private static KeyContainerTypeInfoDB CreateCryptoProKeyContainerTypeInfoDB(string rootPath, bool needDeleted, IList<KeyContainerInfoDB> containers) =>
            new()
            {
                Type = KeyContainerType.CryptoPro,
                KeyContainersInfoDB = containers,
                KeyContainersInfoDeletedDB = needDeleted
                    ? CreateDeletedContainerInfosDB(rootPath)
                    : [],
            };

        private static IList<KeyContainerInfoDB> GenContainersDiskA() =>
            [
                GenKeyContainer("A:\\", containerName: "testKey1.000", certRawData: KSKP_DL),
                GenKeyContainer("A:\\", containerName: "testKey2.000", dateOfCopyUTC: GetNotBefore(-2, -2), certRawData: KSKP_UL_WITH_FIO),
                GenKeyContainer("A:\\", containerName: "testKey3.000", dateOfCopyUTC: GetNotBefore(-3, -3), certRawData: KSKP_UL_WO_FIO),
            ];

        private static IList<KeyContainerInfoDB> GenContainersDiskD() =>
            [
                GenKeyContainer("D:\\", containerName: "testKey4.000", dateOfCopyUTC: GetNotBefore(-4, -4), certRawData: KSKP_FL),
                GenKeyContainer("D:\\", containerName: "testKey5.000", dateOfCopyUTC: GetNotBefore(-5, -5), certRawData: KSKP_IP, isEncrypted: true),
                GenKeyContainer("D:\\", containerName: "testKey6.000", dateOfCopyUTC: GetNotBefore(-6, -6), certRawData: NSKP, isEncrypted: true),
            ];

        private static IList<KeyContainerInfoDB> GenContainersDiskE() =>
            [
                GenKeyContainer("E:\\", containerName: "testKey7.000", isExportable: true),
                GenKeyContainer("E:\\", containerName: "testKey8.000", certRawData: BAD_CERT),
                GenKeyContainer("E:\\", dateOfCopyUTC:  GetNotBefore(-1, -3), isExportable: true),
                GenErrorKeyContainer("E:\\", "KeyContainer.TestError", "errorKey.000"),
            ];

        private static IList<KeyContainerInfoDB> CreateDeletedContainerInfosDB(string rootPath) =>
            [
                GenKeyContainer(ToDeletedStore(rootPath), containerName: "delKey01.000", dateOfCopyUTC: GetNotBefore(-2, -6), certRawData: NSKP),
            ];

        private static string ToDeletedStore(string rootPath) =>
            Path.Combine(rootPath, Txt.DELETED_KEY_DIR_NAME);

        private static DateTime GetNotBefore(int days, int minutes) =>
            DefaultLocaleBeginDate.AddDays(days).AddMinutes(minutes)
                .AddHours(-_utcOffset);

        // for Delete
        private string Delete(string path)
        {
            var deleter = new TestDeleter(_readers);

            return deleter.Delete(path);
        }
    }
}
