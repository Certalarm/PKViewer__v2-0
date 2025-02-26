using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfMvvm.ViewModels.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using WpfMvvm.ViewModels.MainWindow.ThemeSwitcher;


namespace WpfMvvm.ViewModels.MainWindow.StatusBar
{
    public class StatusBarVM: ViewModel
    {
        private ThemeSwitcherVM _themeSwitcherVM;
        private CtrlComboboxVM _langComboboxVM;

        public ThemeSwitcherVM ThemeSwitcherVM
        {
            get => _themeSwitcherVM;
            set => Set(ref _themeSwitcherVM, value);
        }

        public CtrlComboboxVM LangComboboxVM
        {
            get => _langComboboxVM;
            set => Set(ref _langComboboxVM, value);
        }

        #region .ctors
        public StatusBarVM()
        {
            _themeSwitcherVM = new ThemeSwitcherVM();
            _langComboboxVM = CtrlComboboxVM.CreateVMWithOneItem(string.Empty);
        }
        #endregion

        public async Task InitLangVmItemsAsync()
        {
            var items = await GetLangVmItemsAsync()
                .ConfigureAwait(false);
            LangComboboxVM.Items = new(items);
        }

        private static async Task<IEnumerable<CtrlComboboxItemVM>> GetLangVmItemsAsync() =>
            (await LangLibSearcher.FindAsync())
                .Select(x => new CtrlComboboxItemVM(x));
    }
}
