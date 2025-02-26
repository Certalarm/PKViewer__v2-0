using PKInfo.Utility;
using System;
using System.IO;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.Asn1DerHelper;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal static class Asn1DerReader
    {
        private const int KEY_FILE_MIN_LEN = 4;

        public static string ReadFile(string fullFilename, out byte[] fileContent)
        {
            fileContent = [];
            if(!File.Exists(fullFilename))
                return KeyFileNotExist;
            var error = TryReadFile(fullFilename, out fileContent);
            if (error.Length > 0)
                return error;
            error = CheckLen(fileContent);
            if (error.Length > 0)
                fileContent = [];
            return error;
        }

        private static string CheckLen(byte[] fileContent)
        {
            if (fileContent.Length < KEY_FILE_MIN_LEN)
                return KeyFileTooShort;
            if (!IsRequiredContentLen(fileContent))
                return KeyFileBadLen;
            return string.Empty;
        }

        private static string TryReadFile(string fullFileName, out byte[] fileContent)
        {
            fileContent = [];
            var error = string.Empty;
            try
            {
                fileContent = File.ReadAllBytes(fullFileName);
            }
            catch(Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }
    }
}
