using PKInfo.Data.Interface;
using PKInfo.Utility;
using System;
using System.IO;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.CertWriter
{
    internal class FileCertWriter : ICertWriter
    {
        #region .ctors
        public FileCertWriter()
        {
        }
        #endregion

        // Возвращает или ошибку, или полный путь сохраненного файла.
        public string Write(byte[] certRawData, string serial)
        {
            var fullFilePathOrError = FileNamer.GetFullFilePath(serial);
            return fullFilePathOrError.StartsWith(CLRException)
                ? fullFilePathOrError
                : SaveToFile(certRawData, fullFilePathOrError);
        }

        private static string SaveToFile(byte[] certRawData, string fullPath)
        {
            var result = TryCertRawDataSaveToFile(certRawData, fullPath);
            if (string.IsNullOrWhiteSpace(result) && File.Exists(fullPath))
                result = fullPath;
            return string.IsNullOrWhiteSpace(result)
                ? KeyContainerCertSaveUnknownError
                : result;
        }

        private static string TryCertRawDataSaveToFile(byte[] certRawData, string fullPath)
        {
            try
            {
                File.WriteAllBytes(fullPath, certRawData);
            }
            catch (Exception ex)
            {
                return ex.AsError();
            };
            return string.Empty;
        }
    }
}
