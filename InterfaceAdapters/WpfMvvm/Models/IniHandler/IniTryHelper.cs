using System;
using System.Collections.Generic;
using System.IO;
using static WpfMvvm.Models.IniHandler.IniABC;

namespace WpfMvvm.Models.IniHandler
{
    internal static class IniTryHelper
    {
        internal static string TryLoadFromFile(string filename, out IList<string> IniLines)
        {
            string line;
            IniLines = [];
            try
            {
                using StreamReader reader = new StreamReader(filename);
                while ((line = reader.ReadLine()) != null && GetValueLineOrEmpty(line.Trim()).Length > 0)
                    IniLines.Add(line);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        internal static string TrySaveToFile(IniSections sections, string filePath)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(filePath);
                foreach (var section in sections)
                    WriteSection(writer, section);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        private static string GetValueLineOrEmpty(string line) =>
            IsCommentOrEmpty(line)
                ? string.Empty
                : line;

        private static bool IsCommentOrEmpty(string line) =>
           string.IsNullOrEmpty(line) || line.StartsWith(Semicolon) || line.StartsWith(Sharp);

        private static void WriteSection(StreamWriter writer, KeyValuePair<string, IniSection> section)
        {
            writer.WriteLine($"{LeftSquareBracket}{section.Key}{RightSquareBracket}");
            foreach (var pair in section.Value)
                writer.WriteLine($"{pair.Key}{Equally}{pair.Value}");
            writer.WriteLine(); // Empty line between sections
        }
    }
}
