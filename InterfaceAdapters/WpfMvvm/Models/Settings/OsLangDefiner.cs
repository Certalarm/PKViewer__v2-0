using System.Globalization;

namespace WpfMvvm.Models.Settings
{
    internal static class OsLangDefiner
    {
        internal static string GetLang()
        {
            var osLang = CultureInfo.CurrentCulture.Name;
            return SetCapitalFirstLetterOnly(osLang);
        }

        private static string SetCapitalFirstLetterOnly(string value) =>
            char.ToUpper(value[0]) + value.Substring(1).ToLower(); 
    }
}
