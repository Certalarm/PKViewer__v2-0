using System;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Utility
{
    using StringMap = Dictionary<string, string>;

    internal static class Exts
    {
        internal static bool EqualsIgnoreCase(this string value1, string value2) =>
            string.Equals(value1, value2, StringComparison.OrdinalIgnoreCase);

        internal static bool HasKey(this StringMap map, string key) =>
            map.Keys.Any()
            && map.Keys.Contains(key, StringComparer.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(map[key]);

        internal static string AsError(this Exception ex) =>
           $"{CLRException}{Dot}{Space}{ex.Message}";

        internal static string BytesToString(IEnumerable<byte> byteData) =>
            byteData.Any()
                ? NotEmptyBytesToString(byteData)
                : string.Empty;

        private static string NotEmptyBytesToString(IEnumerable<byte> data) =>
            data.Count() > 20
                ? ManyBytesToString(data)
                : FewBytesToString(data);

        private static string ManyBytesToString(IEnumerable<byte> data) =>
            FewBytesToString(data.Take(10))
            + $" {ThreeDot} "
            + FewBytesToString(data.Skip(data.Count() - 10));

        private static string FewBytesToString(IEnumerable<byte> data) =>
            ReplaceMinusToSpace(BitConverter.ToString(data.ToArray()));

        private static string ReplaceMinusToSpace(string line) =>
            line.Replace(oldChar: Minus, newChar: Space);

    }
}
