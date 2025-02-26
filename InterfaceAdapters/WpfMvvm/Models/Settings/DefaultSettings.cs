using System;
using static WpfMvvm.Models.Settings.SettingsKnownParts;

namespace WpfMvvm.Models.Settings
{
    internal static class DefaultSettings
    {
        private static readonly StringComparer __caseFree = StringComparer.OrdinalIgnoreCase;

        private static IniSections _sections;

        internal static IniSections Sections => GetSections();

        private static IniSections GetSections()
        {
            _sections ??= BuildSections();
            return _sections;
        }

        private static IniSections BuildSections() =>
            new (__caseFree)
            {
                { SectionMain, BuildSectionMain() }
            };

        private static IniSection BuildSectionMain() => 
            new (__caseFree)
            {
                { MainLang, OsLangDefiner.GetLang() },
                { MainIsDark, OsThemeDefiner.IsDark() },
                { MainIsShowPubKeyPrefix,  MainIsShowPubKeyPrefixDefault },
                { MainIsDeletedPresent, MainIsDeletedPresentDefault }
            };
    }
}
