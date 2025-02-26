using System;
using System.Collections.Generic;
using System.Diagnostics;
using static WpfMvvm.Models.IniHandler.IniABC;
using static WpfMvvm.Models.IniHandler.IniTryHelper;

namespace WpfMvvm.Models.IniHandler
{
    internal static class IniHandler
    {
        private static readonly StringComparer __caseFree = StringComparer.OrdinalIgnoreCase;

        public static string GetValueOrEmpty(this IniSections sections, string sectionName, string key)
        {
            return sections.ContainsKey(sectionName) && sections[sectionName].ContainsKey(key)
                ? sections[sectionName][key]
                : string.Empty;
        }

        public static void SetValue(this IniSections sections, string sectionName, string key, string value)
        {
            if (sections.ContainsKey(sectionName))
                sections[sectionName][key] = value;
        }

        public static string SaveToFile(this IniSections sections, string filePath)
        {
            var error = TrySaveToFile(sections, filePath);
            if (error.Length > 0)
                Debug.WriteLine(error);
            return error;
        }

        public static string LoadFromFile(this IniSections sections, string filePath)
        {
            var error = TryLoadFromFile(filePath, out var iniLines);
            if (error.Length > 0)
            {
                Debug.WriteLine(error);
                return error;
            }
            string currentSection = null;
            foreach (var line in iniLines)
                LineIntoSection(sections, line, ref currentSection);
            return string.Empty;
        }

        private static void LineIntoSection(IniSections sections, string line, ref string currentSection)
        {
            if (IsSection(line))
            {
                currentSection = SkipFirstAndLastSym(line);
                sections.AddSection(currentSection);
            }
            else
                AddSectionItem(sections, currentSection, line);
        }

        private static void AddSection(this IniSections sections, string sectionName)
        {
            if (!sections.ContainsKey(sectionName))
                sections[sectionName] = new IniSection(__caseFree);
        }

        private static bool IsSection(string line) =>
            line.StartsWith(LeftSquareBracket) && line.EndsWith(RightSquareBracket);

        private static string SkipFirstAndLastSym(string line) =>
            line.Substring(1, line.Length - 2);

        private static void AddSectionItem(IniSections sections, string currentSection, string line)
        {
            if (string.IsNullOrWhiteSpace(currentSection))
                return;
            var pair = ToPair(line);
            if (!string.IsNullOrEmpty(pair.Key))
                sections.SetValue(currentSection, pair.Key, pair.Value);
        }

        private static KeyValuePair<string, string> ToPair(string line)
        {
            var parts = line.Split([Equally], StringSplitOptions.None);
            if (parts.Length != 2)
                return new();
            var key = parts[0].Trim();
            var value = parts[1].Trim();
            return new(key, value);
        }
    }
}
