using PKInfo.Utility;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.CertWriter
{
    internal static class FileNamer
    {
        private const string __cerExtWithDot = ".cer";
        private static readonly Guid __downloadsDirGuid = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        private static readonly string __downloadsDirPath = TryGetDownloadsDirPath();
        private static readonly string __tempDirPath = TryGetTempDirPath();

        internal static string GetFullFilePath(string filename)
        {
            var fullFilePath = BuildFullFilePath(filename);
            return fullFilePath.StartsWith(CLRException)
                ? fullFilePath
                : MatchFullFilePath(filename);
        }

        private static string MatchFullFilePath(string filename)
        {
            string incrementedFileName = GetIncrementedFilename(filename);
            var fullFilePath = BuildFullFilePath(filename);
            while (File.Exists(fullFilePath))
            {
                fullFilePath = BuildFullFilePath(incrementedFileName);
                incrementedFileName = GetIncrementedFilename(incrementedFileName);
            }
            return fullFilePath;
        }

        private static string BuildFullFilePath(string filename)
        {
            var fullDirPath = GetFullDirPath();
            return fullDirPath.StartsWith(CLRException)
                ? fullDirPath
                : System.IO.Path.Combine(fullDirPath, FilenameWithExt(filename));
        }

        private static string GetFullDirPath() =>
            __downloadsDirPath.StartsWith(CLRException)
                ? __tempDirPath
                : __downloadsDirPath;

        private static string FilenameWithExt(string filename) =>
           $"{FilenameForSave(filename)}{__cerExtWithDot}";

        private static string FilenameForSave(string filename) =>
            string.IsNullOrWhiteSpace(filename)
                ? System.IO.Path.GetRandomFileName().Substring(0, 8)
                : filename;

        private static string GetIncrementedFilename(string filename)
        {
            (string prefix, string postfix) = GetPrefixAndPostfix(filename);
            return $"{prefix}{Space}{LeftSquareBracket}{CalcFileCopyNum(postfix)}{RightSquareBracket}";
        }

        private static (string, string) GetPrefixAndPostfix(string filename)
        {
            var parts = filename.Split(Space);
            var postfix = parts.Last();
            bool isLasrPartHasBrackets = postfix.StartsWith($"{LeftSquareBracket}")
                && postfix.EndsWith($"{RightSquareBracket}");
            var prefix = parts.Length > 1 && isLasrPartHasBrackets
                ? string.Join($"{Space}", parts.Take(parts.Length - 1))
                : filename;
            return (prefix, isLasrPartHasBrackets ? postfix : filename);
        }

        private static string CalcFileCopyNum(string strNumWithBrackets)
        {
            var strNum = strNumWithBrackets.TrimStart(LeftSquareBracket).TrimEnd(RightSquareBracket);
            bool isNum = int.TryParse(strNum, out var num);
            return $"{(isNum ? ++num : 1)}";
        }

        private static string TryGetTempDirPath()
        {
            try
            {
                return System.IO.Path.GetTempPath();
            }
            catch (Exception ex)
            {
                return ex.AsError();
            }
        }

        private static string TryGetDownloadsDirPath()
        {
            try
            {
                return SHGetKnownFolderPath(__downloadsDirGuid, 0);
            }
            catch(Exception ex)
            {
                return ex.AsError();
            }
        }

        [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = default);
    }
}
