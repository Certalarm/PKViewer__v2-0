using System;
using System.Windows;

namespace PKViewer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string __langDirName = "lang";

        internal static void AddLangDirPath() =>
            AddAssemblyKnownPath(__langDirName);

        private static void AddAssemblyKnownPath(string dirname) =>
            AppDomain.CurrentDomain.AppendPrivatePath(dirname);
    }
}
