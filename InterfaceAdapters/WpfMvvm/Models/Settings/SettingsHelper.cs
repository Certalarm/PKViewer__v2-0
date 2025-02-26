using WpfMvvm.Models.Settings.Base;
using static WpfMvvm.Models.Settings.SettingsKnownParts;

namespace WpfMvvm.Models.Settings
{
    internal static class SettingsHelper
    {
        internal static string GetMainSectionValue(BaseSettings settings, string key, string defaultValue) =>
            GetSectionValue(settings, SectionMain, key, defaultValue);

        private static string GetSectionValue(BaseSettings settings, string sectionName, string key, string defaultValue)
        {
            var value = settings.GetValue(sectionName, key);
            return value.Length < 1
                ? defaultValue
                : value;
        }
    }
}
