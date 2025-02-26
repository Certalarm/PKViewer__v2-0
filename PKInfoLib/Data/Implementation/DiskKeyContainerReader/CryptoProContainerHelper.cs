using PKInfo.Data.Implementation.DiskKeyContainerDeleter;
using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.Asn1DerReader;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal static class CryptoProContainerHelper
    {
        private const string CPKeyContainerDirPattern = @"^([a-zA-Z0-9-]{8})(\.\d{3})$"; // 8 latin sym or num or minus sign + dot + 3 num
        private const string CPKeyExtensionWithDot = ".key";
        internal const string CPNameKey = "name" + CPKeyExtensionWithDot;
        internal const string CPHeaderKey = "header" + CPKeyExtensionWithDot;
        private const int CPKeyFilesCount = 6;

        public static string ReadCPNameKey(string containerPath, out byte[] fileContent)
        {
            var fullPath = Path.Combine(containerPath, CPNameKey);
            return ReadFile(fullPath, out fileContent);
        }

        public static string ReadCPHeaderKey(string containerPath, out byte[] fileContent)
        {
            var fullPath = Path.Combine(containerPath, CPHeaderKey);
            return ReadFile(fullPath, out fileContent);
        }

        public static string GetContainerPaths(string readerPath, out IEnumerable<string> paths)
        {
            paths = [];
            if (string.IsNullOrEmpty(readerPath))
                return string.Empty;
            var error = TryGetAllDirectories(readerPath, out paths);
            if (!string.IsNullOrEmpty(error))
                return error;
            error = FilterDirectories(paths, out paths);
            return error;
        }

        public static string IsContainerContentValid(string containerPath)
        {
            var error = TryGetKeyFiles(containerPath, out var keyFiles);
            if (!string.IsNullOrEmpty(error))
                return error;
            return IsKeyFilesValid(keyFiles);
        }

        private static string IsKeyFilesValid(string[] keyFiles) =>
            IsKeyFilesCountValid(keyFiles) && IsNeedKeyFilesPresent(keyFiles)
                ? string.Empty
                : Txt.KeyContainerNotValid;

        private static string FilterDirectories(IEnumerable<string> dirs, out IEnumerable<string> filteredDirs)
        {
            filteredDirs = [];
            if (!dirs.Any())
                return Txt.KeyContainersTypeNotFound;
            filteredDirs = dirs
                .Where(x => Regex.IsMatch(DirectoryNamer.GetDirName(x), CPKeyContainerDirPattern));
            return filteredDirs.Any()
                ? string.Empty
                : Txt.KeyContainersTypeNotFound;
        }

        private static string TryGetAllDirectories(string readerPath, out IEnumerable<string> paths)
        {
            paths = [];
            var error = string.Empty;
            try
            {
                paths = Directory.GetDirectories(readerPath, "*.*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }

        private static string TryGetKeyFiles(string dirFullPath, out string[] keyFiles)
        {
            keyFiles = [];
            var error = string.Empty;
            var searchPattern = $"*{CPKeyExtensionWithDot}";
            try
            {
                keyFiles = Directory.GetFiles(dirFullPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch(Exception ex)
            {
                error = ex.AsError();
            }
            return error;
        }

        private static bool IsKeyFilesCountValid(string[] fullFileNames) =>
            !(fullFileNames.Length < CPKeyFilesCount);

        private static bool IsNeedKeyFilesPresent(string[] fullFileNames) =>
            IsFilePresent(CPNameKey, fullFileNames) && IsFilePresent(CPHeaderKey, fullFileNames);

        private static bool IsFilePresent(string fileName, string[] fullFileNames) =>
            fullFileNames.Any(x => x.EndsWith(fileName));
    }
}
