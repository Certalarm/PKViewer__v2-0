using System;
using System.ComponentModel;
using System.Windows;
using WpfMvvm.ViewModels.MainWindow;
using static WpfMvvm.ViewModels.MainWindow.MainWindowVMsDetector;

namespace PKViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.AddLangDirPath();
            InitializeComponent();
            Dispatcher.Invoke(async () => await GetMainWindowVM()?.ExecuteStartBehaviorsAsync());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            GetMainWindowVM()?.ExecuteShutdownBehaviors();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            MainWindowVM.SwitchTheme();
            MainWindowVM.AutoSizeColumnsWidth();
        }

        private void ColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GetMainWindowVM()?.ExecuteSortMainLVBehaviors(e.OriginalSource);
        }

        private void ColumnHeaderSizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            var parameters = new object[] { sender, e.NewSize.Width };
            GetMainWindowVM()?.ExecuteHeaderWidMainLVBehaviors(parameters);
            GetMainWindowVM()?.ExecuteStartBehaviors();
        }
    }
}
