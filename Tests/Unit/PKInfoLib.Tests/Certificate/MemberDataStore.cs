using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using static PKInfo.Utility.Txt;

namespace PKInfoLib.Tests.Certificate
{
    internal static class MemberDataStore
    {
        private const string __Key1 = "A:\\testKey1.000";   // КСКП для Должностного лица
        private const string __Key2 = "A:\\testKey2.000";   // КСКП для Юр. лица
        private const string __Key3 = "A:\\testKey3.000";   // КСКП для Юр. лица без ФИО
        private const string __Key4 = "D:\\testKey4.000";   // КСКП для физ. лица
        private const string __Key5 = "D:\\testKey5.000";   // КСКП для ИП
        private const string __Key6 = "D:\\testKey6.000";   // НСКП
        private const string __Key7 = "E:\\testKey7.000";   // Контейнер без сертификата
        private const string __Key8 = "E:\\testKey8.000";   // Контейнер с неправильными данными в сертификате
        private const string __Key10 = "E:\\badKey10.000";  // Несуществующий контейнер

        internal static List<object[]> DataWithErrors() =>
        [ // format: containerPath, Error
            [ __Key7, KeyContainerCertNotFound ],    // Контейнер без сертификата
            [ __Key8, CLRException ],                // Контейнер с неправильными данными в сертификате
            [ __Key10, KeyContainerNotExist ],       // Несуществующий контейнер
        ];

        internal static List<object[]> DataForCheckTypes() =>
        [ // format: containerPath, certType, certOwnerType
            [ __Key1, CertType.Qualifed, CertOwnerType.ForDL ],         // КСКП для Должностного лица
            [ __Key2, CertType.Qualifed, CertOwnerType.ForUL ],         // КСКП для Юр. лица
            [ __Key3, CertType.Qualifed, CertOwnerType.ForULwoFIO ],    // КСКП для Юр. лица без ФИО
            [ __Key4, CertType.Qualifed, CertOwnerType.ForFL ],         // КСКП для физ. лица
            [ __Key5, CertType.Qualifed, CertOwnerType.ForIP ],         // КСКП для ИП
            [ __Key6, CertType.Unqualifed, CertOwnerType.Unknown ],     // НСКП
            [ __Key7, CertType.Unknown, CertOwnerType.Unknown ],        // Контейнер без сертификата
            [ __Key8, CertType.Unknown, CertOwnerType.Unknown ],        // Контейнер с неправильными данными в сертификате
            [ __Key10, CertType.Unknown, CertOwnerType.Unknown ],       // Несуществующий контейнер
        ];

        internal static List<object[]> DataForCheckCertSomeV1Fields() =>
        [ // format: containerPath, Serial, DateNotBefore, DateNotAfter
            [ __Key1, "0001", new DateTime(2024, 06, 02, 17, 3, 0),  new DateTime(2025, 09, 02, 17, 3, 0) ],  // КСКП для Должностного лица
            [ __Key2, "0002", new DateTime(2024, 05, 31, 17, 1, 0),  new DateTime(2025, 08, 31, 17, 1, 0) ],  // КСКП для Юр. лица
            [ __Key3, "0003", new DateTime(2024, 05, 30, 17, 0, 0),  new DateTime(2025, 08, 30, 17, 0, 0) ],  // КСКП для Юр. лица без ФИО
            [ __Key4, "0004", new DateTime(2024, 05, 29, 16, 59, 0),  new DateTime(2025, 08, 29, 16, 59, 0) ],// КСКП для физ. лица
            [ __Key5, "0005", new DateTime(2024, 05, 28, 16, 58, 0),  new DateTime(2025, 08, 28, 16, 58, 0) ],// КСКП для ИП
            [ __Key6, "0006", new DateTime(2024, 05, 27, 16, 57, 0),  new DateTime(2025, 08, 27, 16, 57, 0) ],// НСКП
            [ __Key7, null, null,  null ],      // Контейнер без сертификата
            [ __Key8, null, null,  null ],      // Контейнер с неправильными данными в сертификате
            [ __Key10, null, null,  null ],     // Несуществующий контейнер
        ];

        internal static List<object[]> DataForCheckSubjectFields() =>
        [ // format: containerPath, O, OU, owner (SN + G or CN), T, E
            [ __Key1, "Тестовое управление", "Тестовый отдел №1", "Тестиков Тест Тестович", "шеф тестов", "testikoff@test.com" ],   // КСКП для Должностного лица
            [ __Key2, "Тестовое управление", "Тестовый отдел №1", "Тестиков Тест Тестович", "шеф тестов", "testikoff@test.com" ],   // КСКП для Юр. лица
            [ __Key3, "Тестовое управление", "Тестовый отдел №1", "Тестовое управление", null, "testikoff@test.com" ],              // КСКП для Юр. лица без ФИО
            [ __Key4, null, null, "Тестиков Тест Тестович", null, "testikoff@test.com" ],                                           // КСКП для физ. лица
            [ __Key5, null, null, "Тестиков Тест Тестович", null, "testikoff@test.com" ],                                           // КСКП для ИП
            [ __Key6, "Testovoe Upravlenie", "Testovy Otdel #1", "Testovoe Upravlenie", null, "podymahino@mail.ru" ],               // НСКП
            [ __Key7, null, null,  null, null, null ],      // Контейнер без сертификата
            [ __Key8, null, null,  null, null, null ],      // Контейнер с неправильными данными в сертификате
            [ __Key10, null, null,  null, null, null ],     // Несуществующий контейнер
        ];

        internal static List<object[]> DataForCheckIssuerFields() =>
        [ // format: containerPath, CN
            [ __Key1, "Тестовый УЦ" ],  // КСКП для Должностного лица
            [ __Key2, "Тестовый УЦ" ],  // КСКП для Юр. лица
            [ __Key3, "Тестовый УЦ" ],  // КСКП для Юр. лица без ФИО
            [ __Key4, "Тестовый УЦ" ],  // КСКП для физ. лица
            [ __Key5, "Тестовый УЦ" ],  // КСКП для ИП
            [ __Key6, "Testovy CA" ],   // НСКП
            [ __Key7, null ],           // Контейнер без сертификата
            [ __Key8, null ],           // Контейнер с неправильными данными в сертификате
            [ __Key10, null ],          // Несуществующий контейнер
        ];
    }
}
