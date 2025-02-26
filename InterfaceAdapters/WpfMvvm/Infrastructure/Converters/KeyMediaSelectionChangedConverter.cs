using System.Linq;
using System.Threading.Tasks;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using WpfMvvm.ViewModels.MainWindow.TopPanel;

namespace WpfMvvm.Infrastructure.Converters
{
    public class KeyMediaSelectionChangedConverter : SelectionChangedConverter
    {
        protected override async void ExecuteCommand(int index)
        {
            if (index < 0)
                return;

            var topPanelVM = GetTopPanelVM();
            var readersVM = topPanelVM.ReaderComboBoxVM;

            if (IsBadItems(readersVM))
                return;

            await UpdateListViewItems(readersVM, topPanelVM, index);
        }

        private bool IsBadItems(CtrlComboboxVM readersVM) =>
            IsBadReaderItems(readersVM) || IsBadKeyMediaItems();

        private static bool IsBadReaderItems(CtrlComboboxVM readersVM) =>
            readersVM.Items == null || !readersVM.Items.Any();

        private bool IsBadKeyMediaItems() =>
            _ctrlComboboxVM.Items == null || !_ctrlComboboxVM.Items.Any();

        private async Task UpdateListViewItems(CtrlComboboxVM readersVM, TopPanelVM topPanelVM, int index)
        {
            var listViewVM = GetMainListViewVM();
            var readerType = readersVM.Items[readersVM.SelectedIndex].Title;
            var keyMediaRootPath = _ctrlComboboxVM.Items[index].Title;
            await listViewVM.UpdateItemsAsync(readerType, keyMediaRootPath, topPanelVM.IsDeletedPresent, false);
        }
    }
}
