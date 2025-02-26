using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfMvvm.Infrastructure.Commands;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class FlagKeyToControlHelper
    {
        private const string __minus = "-";
        private const string __title = "Title";
        private const string __default = "Default";

        internal static KeyValuePair<string, ContentControl> FindFlag(string lang)
        {
            var flagsDict = FindResource(LoadFlagsCommand.__xamlName);
            return flagsDict == null
                ? CreateEmptyPair()
                : BuildFlagPair(flagsDict, lang);
        }

        internal static KeyValuePair<string, ContentControl> CreateEmptyPair() =>
            new(string.Empty, null);

        private static KeyValuePair<string, ContentControl> BuildFlagPair(ResourceDictionary flagsDict, string lang)
        {
            var flagKey = ToFlagKey(lang);
            var title = GetTitle(flagsDict, flagKey);
            var flag = GetFlag(flagsDict, flagKey);
            return new KeyValuePair<string, ContentControl>(title, flag);
        }

        private static string ToFlagKey(string lang) => lang.Replace(__minus, string.Empty);

        private static string GetTitle(ResourceDictionary flagsDict, string flagKey) =>
            flagsDict[GetLangTitle(flagKey)] as string ?? GetDefaultTitle();

        private static string GetLangTitle(string flagKey) => $"{flagKey}{__title}";

        private static string GetDefaultTitle() => $"{__minus}{__minus}{__minus}";

        private static ContentControl GetFlag(ResourceDictionary flagsDict, string flagKey) =>
            flagsDict[flagKey] as ContentControl ?? GetDefaultFlag(flagsDict);

        private static ContentControl GetDefaultFlag(ResourceDictionary flagsDict) =>
            flagsDict[__default] as ContentControl;
    }
}
