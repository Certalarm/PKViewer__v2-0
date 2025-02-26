using System.Linq;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ReaderSelectionChangedConverter : SelectionChangedConverter
    {
        protected override async void ExecuteCommand(int index)
        {
            if(index < 0)
                return;

            var vm = GetTopPanelVM();
            if (vm.IsSkipSelectedIndex || IsBadItems())
                return;

            await vm.InitKeyMediasAsync(_ctrlComboboxVM.Items[index].Title);
        }

        private bool IsBadItems() =>
            _ctrlComboboxVM.Items == null || !_ctrlComboboxVM.Items.Any();
    }
}
