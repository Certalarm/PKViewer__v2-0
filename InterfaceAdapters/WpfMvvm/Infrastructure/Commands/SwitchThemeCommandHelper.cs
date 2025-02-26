using System.Windows;
using static WpfMvvm.Infrastructure.Commands.SwitchCommandHelper;
using static WpfMvvm.ViewModels.MainWindow.Win32Helper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal class SwitchThemeCommandHelper
    {
        private const string __uriPostfix = "rsrc/Brushes/";
        private const string __dark = "Dark";
        private const string __light = "Light";
        private const string __themeXaml = "Theme.xaml";

        internal static void SwitchTheme(bool isDark)
        {
            SwitchResourceTheme(isDark);
            SwitchMainWindowSystemTheme(isDark); // invalidate Window TitleBar Colors
        }

        private static void SwitchResourceTheme(bool isDark)
        {
            string xamlName = ToResourceThemeName(isDark);
            RemoveResource(xamlName);
            AddResource(xamlName, __uriPostfix);
        }

        private static string ToResourceThemeName(bool isDark) =>
            $"{(isDark ? __dark : __light)}{__themeXaml}";

        private static void SwitchMainWindowSystemTheme(bool isDark)
        {
            var window = Application.Current.MainWindow;
            SwitchWindowSystemTheme(isDark, window);
        }
    }
}
