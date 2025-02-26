using PKInfo.Domain.Entity;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.DataSource;
using WpfMvvm.ViewModels.Base;
using WpfMvvm.ViewModels.MainWindow.MainLV;
using WpfMvvm.ViewModels.MainWindow.StatusBar;
using WpfMvvm.ViewModels.MainWindow.TopPanel;
using static WpfMvvm.ViewModels.MainWindow.ShutdownBehaviorsHelper;
using static WpfMvvm.ViewModels.MainWindow.SortMainLVBehaviorsHelper;
using static WpfMvvm.ViewModels.MainWindow.StartBehaviorsHelper;
using static WpfMvvm.ViewModels.MainWindow.Win32Helper;

namespace WpfMvvm.ViewModels.MainWindow
{
    public class MainWindowVM : ViewModel
    {
        private readonly ImageSource _windowIcon;
        private SelfInfo _title;

        private StatusBarVM _statusBarVM;
        private MainListViewVM _mainListViewVM;
        private TopPanelVM _topPanelVM;

        public ImageSource WindowIcon => _windowIcon;

        public SelfInfo WindowTitle => _title;

        public StatusBarVM StatusBarVM
        {
            get => _statusBarVM;
            set => Set(ref _statusBarVM, value);
        }

        public MainListViewVM MainListViewVM
        {
            get => _mainListViewVM;
            set => Set(ref _mainListViewVM, value);
        }

        public TopPanelVM TopPanelVM
        {
            get => _topPanelVM;
            set => Set(ref _topPanelVM, value);
        }

        #region .ctors
        public MainWindowVM()
        {
            _statusBarVM = new();
            _topPanelVM = new();
            _mainListViewVM = new();
            _windowIcon = GetIcon();
            InitTitle(); 
        }
        #endregion

        private void InitTitle()
        {
            var assemblyName = AppDomain.CurrentDomain.FriendlyName;
            _title = DataModelsLoader.GetSelfInfo(assemblyName);
        }

        public static void AutoSizeColumnsWidth()
        {
            var gridView = GetMainListView().View as GridView;
            foreach (var column in gridView.Columns)
            {
                column.Width = 0;
                column.Width = double.NaN;
            }
        }

        public static void SwitchTheme() => SetTheme();

        public void ExecuteShutdownBehaviors() => ShutdownBehaviors(this);

        public void ExecuteSortMainLVBehaviors(object clickedHeader) => SortMainLV(clickedHeader);

        public void ExecuteHeaderWidMainLVBehaviors(object[] parameters)
        {
            ICommand cmd = new ListViewHeaderWidthCommand();
            cmd.Execute(parameters);
        }

        public async Task InitFieldsAsync()
        {
            await StatusBarVM.InitLangVmItemsAsync();
            await TopPanelVM.InitFieldsAsync();
        }

        public async Task ExecuteStartBehaviorsAsync() => await StartBehaviorsAsync(this);
        
        public void ExecuteStartBehaviors() => StartBehaviors(this);
    }
}
