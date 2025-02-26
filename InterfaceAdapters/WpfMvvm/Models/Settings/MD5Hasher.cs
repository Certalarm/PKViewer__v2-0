using System;
using System.Security.Cryptography;
using System.Text;

namespace WpfMvvm.Models.Settings
{
    internal static class MD5Hasher
    {
        private const string __minus = "-";

        internal static string CalcHash(IniSections sections)
        {
            var bytes = StringToBytes(SectionsToString(sections));

            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(bytes);
            return BytesToString(hashBytes);
        }

        private static byte[] StringToBytes(string value) =>
            Encoding.UTF8.GetBytes(value);

        private static string SectionsToString(IniSections sections)
        {
            string result = string.Empty;
            foreach(var section in sections)
                result += $"{section.Key}{SectionValuesToString(section.Value)}";
            return result;
        }

        private static string SectionValuesToString(IniSection section)
        {
            string result = string.Empty;
            foreach (var item in section)
                result += item.Value;
            return result;
        }

        private static string BytesToString(byte[] bytes) =>
            Normalize(BitConverter.ToString(bytes));

        private static string Normalize(string value) =>
            value.Replace(__minus, string.Empty).ToLower();
    }
}
