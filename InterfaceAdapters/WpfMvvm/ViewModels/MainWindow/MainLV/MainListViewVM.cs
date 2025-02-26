using PKInfo.Domain.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfMvvm.DataSource;
using WpfMvvm.ViewModels.Base;
using WpfMvvm.ViewModels.KeyContainerItem;

namespace WpfMvvm.ViewModels.MainWindow.MainLV
{
    public class MainListViewVM : ViewModel
    {
        private IList<KeyContainerItemVM> _cachedNoFilteredItems = [];
        
        private bool _isReady;
        private ObservableCollection<KeyContainerItemVM> _items = [];
        private int _itemsCheckedCount;

        public bool IsReady
        {
            get => _isReady;
            set => Set(ref _isReady, value);
        }

        public ObservableCollection<KeyContainerItemVM> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public int ItemsCheckedCount
        {
            get => _itemsCheckedCount;
            set => Set(ref _itemsCheckedCount, value);
        }

        public IEnumerable<string> AllPaths =>
            Items
                .Select(x => x.Path);

        public IEnumerable<string> CheckedPaths =>
            Items
                .Where(x => x.IsChecked)
                .Select(x => x.Path);

        public void UpdateCheckedCount() => 
            ItemsCheckedCount = Items.Count(x => x.IsChecked);

        public void FilterItems(bool isDeletedPresent)
        {
            UpdateFilteredItems(isDeletedPresent);
            UpdateCheckedCount();
        }

        public async Task UpdateItemsAsync(string readerType, string keyMediaRootPath, bool isDeletedPresent, bool needUpdate)
        {
            ShowPreloader();
            await UpdateNoFilteredItemsAsync(readerType, keyMediaRootPath, needUpdate);
            UpdateFilteredItems(isDeletedPresent);
            HidePreloader();
        }

        internal void ShowPreloader()
        {
            if (IsReady)
                Items.Clear();
            IsReady = false;
        }

        internal void HidePreloader()
        {
            IsReady = true;
        }

        private void UpdateFilteredItems(bool isDeletedPresent)
        {
            Items.Clear();
            var items = isDeletedPresent
                ? _cachedNoFilteredItems
                : _cachedNoFilteredItems
                    .Where(x => x.NameHolder.IsDeleted != true);
            if (items.Any() && !Items.Any())
                foreach (var item in items)
                    Items.Add(item);
        }

        private async Task UpdateNoFilteredItemsAsync(string readerType, string keyMediaRootPath, bool needUpdate)
        {
            _cachedNoFilteredItems.Clear();
            _cachedNoFilteredItems = new List<KeyContainerItemVM>(
                await CreateItemsAsync(readerType, keyMediaRootPath, needUpdate));
        }

        private static async Task<IEnumerable<KeyContainerItemVM>> CreateItemsAsync(
            string readerType, string keyMediaRootPath, bool needUpdate)
        {
            var noFilteredItems =
                await GetNoFilteredItemsAsync(readerType, keyMediaRootPath, needUpdate);
            return noFilteredItems
                .Select(KeyContainerItemVM.Create);
        }

        private static async Task<IList<KeyContainerSnapshotInfo>> GetNoFilteredItemsAsync(
            string readerType, string keyMediaRootPath, bool needUpdate)
            => (readerType, keyMediaRootPath) switch
            {
                (__allReaders, __allKeysMedia) => await DataModelsLoader.GetAllKeyContainersAsync(needUpdate),
                (__allReaders, _) => await DataModelsLoader.GetKeyContainersByMediaPathAsync(keyMediaRootPath),
                (_, __allKeysMedia) => await DataModelsLoader.GetKeyContainersByReaderTypeAsync(readerType),
                (_, _) => await DataModelsLoader.GetKeyContainersByMediaPathAsync(keyMediaRootPath),
            };
    }
}
