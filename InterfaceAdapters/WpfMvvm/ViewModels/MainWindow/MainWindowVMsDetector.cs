using WpfMvvm.ViewModels.MainWindow.MainLV;
using WpfMvvm.ViewModels.MainWindow.TopPanel;

namespace WpfMvvm.ViewModels.MainWindow
{
    public static class MainWindowVMsDetector
    {
        private static MainWindowVM _mainVM;

        public static MainWindowVM GetMainWindowVM()
        {
            _mainVM ??= GetMainWindow().DataContext as MainWindowVM;
            return _mainVM;
        }

        internal static bool IsDarkTheme() =>
            GetMainWindowVM().StatusBarVM.ThemeSwitcherVM.IsChecked;

        internal static TopPanelVM GetTopPanelVM() =>
            GetMainWindowVM().TopPanelVM;

        internal static MainListViewVM GetMainListViewVM() =>
            GetMainWindowVM().MainListViewVM;
    }
}
