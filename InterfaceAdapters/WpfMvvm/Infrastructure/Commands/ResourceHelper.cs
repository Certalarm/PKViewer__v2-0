using System.Linq;
using System.Windows;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class ResourceHelper
    {
        internal static string GetValue(ResourceDictionary dict, object key) =>
            dict?[key] as string ?? (string)key;

        internal static ResourceDictionary FindLangDict() =>
            FindResource(SwitchLangCommand.__xamlName);

        internal static bool HasKey(this ResourceDictionary dict, string key) =>
            dict?.Keys
                ?.OfType<string>()
                ?.Contains(key)
                ?? false;

        internal static ResourceDictionary FindResource(string xamlName) =>
            Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(x => x.Source.OriginalString.EndsWith(xamlName));
    }
}
