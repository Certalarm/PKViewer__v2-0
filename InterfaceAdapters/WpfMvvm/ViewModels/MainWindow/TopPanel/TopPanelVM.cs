using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfMvvm.DataSource;
using WpfMvvm.Models.Settings;
using WpfMvvm.ViewModels.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using static WpfMvvm.Infrastructure.Converters.KeyMediaSelectionChangedHelper;
using static WpfMvvm.Models.Settings.SettingsKnownParts;

namespace WpfMvvm.ViewModels.MainWindow.TopPanel
{
    public class TopPanelVM : ViewModel
    {
        private CtrlComboboxVM _readerComboBoxVM;
        private CtrlComboboxVM _keyMediaComboBoxVM;
        private bool _isPublicKeyPrefixPresent;
        private bool _isDeletedPresent;

        public bool IsSkipSelectedIndex { get; private set; }

        public CtrlComboboxVM ReaderComboBoxVM
        {
            get => _readerComboBoxVM;
            set => Set(ref _readerComboBoxVM, value);
        }

        public CtrlComboboxVM KeyMediaComboBoxVM
        {
            get => _keyMediaComboBoxVM;
            set => Set(ref _keyMediaComboBoxVM, value);
        }

        public bool IsDeletedPresent
        {
            get => _isDeletedPresent;
            set => Set(ref _isDeletedPresent, value);
        }

        public bool IsPublicKeyPrefixPresent
        {
            get => _isPublicKeyPrefixPresent;
            set => Set(ref _isPublicKeyPrefixPresent, value);
        }

        #region .ctors
        public TopPanelVM()
        {
            InitComboboxVMs();
            InitBoolProperties();
        }
        #endregion

        public void Refresh()
        {
            IsSkipSelectedIndex = true;
            ReaderComboBoxVM.Refresh();
            KeyMediaComboBoxVM.Refresh();
            IsSkipSelectedIndex = false;
        }

        public async Task InitFieldsAsync()
        {
            await InitReadersAsync();
            if (ReaderComboBoxVM.Items != null && ReaderComboBoxVM.Items.Any())
                await InitKeyMediasAsync(ReaderComboBoxVM.Items[0].Title);
        }

        public async Task InitKeyMediasAsync(string readerType)
        {
            if (string.IsNullOrWhiteSpace(readerType))
                return;
            var allRootPaths = await GetAllRootPathsAsync(readerType);
            KeyMediaComboBoxVM.Items = new(ToItemVMs(allRootPaths));
            KeyMediaComboBoxVM.SelectedIndex = 0;
        }

        private void InitComboboxVMs()
        {
            ReaderComboBoxVM = CtrlComboboxVM.CreateVMWithOneItem(__allReaders);
            KeyMediaComboBoxVM = CtrlComboboxVM.CreateVMWithOneItem(__allKeysMedia);
        }

        private void InitBoolProperties()
        {
            var sectionMain = DefaultSettings.Sections[SectionMain];
            IsPublicKeyPrefixPresent = Convert.ToBoolean(sectionMain[MainIsShowPubKeyPrefix]);
            IsDeletedPresent = Convert.ToBoolean(sectionMain[MainIsDeletedPresent]);
        }

        private async Task InitReadersAsync()
        {
            var typeReaders = await DataModelsLoader.GetTypesReaderWithKeyMediaAsync(true);
            typeReaders = [__allReaders, .. typeReaders];
            ReaderComboBoxVM.Items = new(ToItemVMs(typeReaders));
            ReaderComboBoxVM.SelectedIndex = 0;
        }

        private async Task<IEnumerable<string>> GetAllRootPathsAsync(string readerType)
        {
            IEnumerable<string> allRootPaths = [__allKeysMedia];
            var rootPaths = (await GetRootPathsAsync(readerType));
            if (rootPaths.Any())
                allRootPaths = [.. allRootPaths, .. rootPaths];
            return allRootPaths;
        }

        private static List<CtrlComboboxItemVM> ToItemVMs(IEnumerable<string> values) =>
            values
                .Select(ToItemVM)
                .ToList();

        private static CtrlComboboxItemVM ToItemVM(string value) =>
             new (value);
    }
}
