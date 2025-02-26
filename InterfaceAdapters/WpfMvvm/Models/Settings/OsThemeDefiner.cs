using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace WpfMvvm.Models.Settings
{
    internal static class OsThemeDefiner
    {
        private const string __regKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string __regParam = "AppsUseLightTheme";

        internal static string IsDark()
        {
            var error = TryIsDarkEnabled(out var isDark);
            if (error.Length > 0)
                Debug.WriteLine(error);
            return isDark.ToString();
        }
        
        private static string TryIsDarkEnabled(out bool isDark)
        {
            isDark = false;
            var readOnly = RegistryKeyPermissionCheck.ReadSubTree;
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(__regKey, readOnly);
                var value = key?.GetValue(__regParam);
                isDark = value != null && (int)value == 0;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
    }
}
