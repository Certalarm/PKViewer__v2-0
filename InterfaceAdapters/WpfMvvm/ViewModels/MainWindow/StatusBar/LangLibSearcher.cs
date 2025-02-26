using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WpfMvvm.ViewModels.MainWindow.StatusBar
{
    internal static class LangLibSearcher
    {
        private const string __langDirName = "lang";
        private const string __minus = "-";
        private const string __dllExt = "dll";
        private const string __searchPattern = $"*.{__dllExt}";

        internal static async Task<IEnumerable<string>> FindAsync() =>
            await Task.Run(() => FindInDirectory(GetFullPathSearchDir()))
                .ConfigureAwait(false);

        private static string GetFullPathSearchDir() =>
            Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.FriendlyName), __langDirName);

        private static IEnumerable<string> FindInDirectory(string fullDirPath)
        {
            foreach (var dllName in TryGetFilesOrEmpty(fullDirPath))
            {
                var filenameWoExt = Path.GetFileNameWithoutExtension(dllName);
                if (filenameWoExt.IndexOf(__minus) == 2 && filenameWoExt.Length == 5)
                    yield return filenameWoExt;
            }
        }

        private static IEnumerable<string> TryGetFilesOrEmpty(string dirFullPath)
        {
            try
            {
                return Directory.EnumerateFiles(dirFullPath, __searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return [];
        }
    }
}
