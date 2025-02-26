using PKInfo.Data.DTO;
using System;
using static PKInfoLib.Tests.DAL.Generator.CertDateCorrector;
using static PKInfoLib.Tests.DAL.Generator.CPKeyContainerInfoDBGenerator;

namespace PKInfoLib.Tests.DAL.Generator
{
    internal static class CPKeyContainerInfoDBHelper
    {
        internal static KeyContainerInfoDB GenErrorKeyContainer(
            string rootPath, 
            string error, 
            string containerName = default
            ) =>
            new()
            {
                Error = error,
                Path = GenPath(rootPath, containerName),
            };

        internal static KeyContainerInfoDB GenKeyContainer(
            string rootPath,
            string containerName = default,
            string name = default,
            DateTime? dateOfCopyUTC = null,
            byte[] publicKey = default,
            byte[] certRawData = default,
            bool isEncrypted = false,
            bool? isExportable = default
            ) => 
            new()
            {
                Path = GenPath(rootPath, containerName),
                Name = name == default
                    ? GenNameContainer()
                    : name,
                DateOfCopyUTC = DefineDateOfCopy(dateOfCopyUTC),
                DateNotAfterUTC = GenDateNotAfter(DefineDateOfCopy(dateOfCopyUTC)),
                PublicKey = publicKey == default
                    ? GenPublicKey()
                    : publicKey,
                CertRawData = certRawData is null
                    ? []
                    : ChangeDateOfCopyInCert(certRawData, dateOfCopyUTC),
                IsEncrypted = isEncrypted,
                IsExportable = isExportable,
            };

        private static DateTime DefineDateOfCopy(DateTime? newDate) =>
            newDate is null 
                ? GenDateOfCopy(-5, -5) 
                : (DateTime)newDate;

        private static byte[] ChangeDateOfCopyInCert(byte[] bytes, DateTime? newDate) =>
            ChangeDatesInCert(bytes, newDate is null ? GenDateOfCopy() : (DateTime)newDate);
    }
}
