using System;
using System.Diagnostics;
using System.Windows;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public static class SwitchCommandHelper
    {
        private const string __uriPrefix = "pack://application:,,,/";

        internal static void RemoveResource(string xamlName)
        {
            var dict = FindResource(xamlName);
            if (dict != null)
                Application.Current.Resources.MergedDictionaries.Remove(dict);
        }

        internal static void AddResource(string xamlName, string uriPostfix, string dllNameWoExt = default)
        {
            var uriString = BuildUriString(xamlName, uriPostfix, dllNameWoExt);
            var dict = CreateResource(uriString);
            if (dict != null)
                Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private static ResourceDictionary CreateResource(string uriString)
        {
            var error = TryGetUri(uriString, out var source);
            ResourceDictionary dict = null;
            if (error.Length == 0)
                error = TryCreateResource(source, out dict);
            if (error.Length > 0)
                Debug.WriteLine(error);
            return dict;
        }

        private static string TryCreateResource(Uri source, out ResourceDictionary dict)
        {
            dict = null;
            try
            {
                dict = new() 
                { 
                    Source = source 
                };
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        private static string TryGetUri(string uriString, out Uri uri)
        {
            uri = null;
            try
            {
                uri = new Uri(uriString, UriKind.Absolute);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        private static string BuildUriString(string xamlName, string uriPostfix, string lang) =>
          $"{__uriPrefix}{lang}{uriPostfix}{xamlName}";
    }
}
