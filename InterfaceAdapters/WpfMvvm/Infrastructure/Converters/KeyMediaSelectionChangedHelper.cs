using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WpfMvvm.DataSource;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class KeyMediaSelectionChangedHelper
    {
        private const double __kilo = 1024.0;
        private const double __mega = __kilo * __kilo;
        private const char __space = ' ';

        internal static async Task<IEnumerable<string>> GetRootPathsAsync(string strReaderType) =>
            Enum.TryParse(strReaderType, out ReaderType readerType)
                ? await DataModelsLoader.GetReaderMediaRootPathsAsync(readerType)
                : await DataModelsLoader.GetAllMediaRootPathsAsync();

        internal static string GetKeyMediaDisplayName(KeyMediaSnapshotInfo mediaInfo)
        {
            var langDict = FindLangDict();
            var rootPath = RootPath(mediaInfo.RootPath);
            var label = Label(mediaInfo.Label, mediaInfo.Type, langDict);
            var size = Size(mediaInfo.Size, langDict);
            if (!string.IsNullOrWhiteSpace(size))
                label += __space;
            return $"{rootPath}{label}{size}";
        }

        private static string RootPath(string rootPath) =>
            string.IsNullOrWhiteSpace(rootPath)
                ? string.Empty
                : $"[{rootPath.TrimEnd(Path.DirectorySeparatorChar)}]{__space}";

        private static string Label(string label, KeyMediaType type, ResourceDictionary langDict) =>
            string.IsNullOrWhiteSpace(label)
                ? GetValue(langDict, type.ToString())
                : label;

        private static string Size(long sizeInB, ResourceDictionary langDict) =>
            sizeInB > 0
                ? $"({TruncSize(sizeInB, langDict)})"
                : string.Empty;

        private static string TruncSize(long sizeInB, ResourceDictionary langDict)
        {
            var sizeInMb = BytesToMb(sizeInB);
            return IsLargerThanOneGb(sizeInMb)
                ? TruncIfLargeThenOneGb(sizeInMb, langDict)
                : TruncToMb(sizeInMb, langDict);
        }

        private static bool IsLargerThanOneGb(double sizeInMb) => sizeInMb >= __kilo;

        private static string TruncIfLargeThenOneGb(double sizeInMb, ResourceDictionary langDict) =>
            IsLargerThanOneTb(sizeInMb)
               ? TruncToTb(sizeInMb, langDict)
               : TruncToGb(sizeInMb, langDict);

        private static bool IsLargerThanOneTb(double sizeInMb) => sizeInMb >= __mega;

        private static string TruncToTb(double sizeInMb, ResourceDictionary langDict) =>
            $"{RoundByTwoDigits(MbToTb(sizeInMb)):N}{__space}{GetValue(langDict, __tB)}";

        private static string TruncToGb(double sizeInMb, ResourceDictionary langDict) =>
            $"{RoundByTwoDigits(MbToGb(sizeInMb)):N}{__space}{GetValue(langDict, __gB)}";

        private static string TruncToMb(double sizeInMb, ResourceDictionary langDict) =>
            $"{RoundByTwoDigits(sizeInMb):N}{__space}{GetValue(langDict, __mB)}";

        private static double MbToTb(double sizeInMb) =>
            sizeInMb / __mega;
        
        private static double MbToGb(double sizeInMb) =>
            sizeInMb / __kilo;

        private static double BytesToMb(long sizeInB) =>
            sizeInB / __mega;

        private static double RoundByTwoDigits(double value) =>
            Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
