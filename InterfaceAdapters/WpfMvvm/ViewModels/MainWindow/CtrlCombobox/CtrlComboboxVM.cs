using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfMvvm.ViewModels.Base;

namespace WpfMvvm.ViewModels.MainWindow.CtrlCombobox
{
    public class CtrlComboboxVM: ViewModel
    {
        private int _selectedIndex;
        private ObservableCollection<CtrlComboboxItemVM> _items;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

        public ObservableCollection<CtrlComboboxItemVM> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        #region .ctors
        public CtrlComboboxVM()
        {
            Items = [];
        }
        #endregion

        public static CtrlComboboxVM CreateVMWithOneItem(string itemTitle) =>
            new()
            {
                Items = new([new(itemTitle)])
            };

        public void Refresh()
        {
            var selectedIndex = SelectedIndex;
            var tmpItems = new List<CtrlComboboxItemVM>(Items);
            Items.Add(new(string.Empty));
            SelectedIndex = Items.Count - 1;
            Items = new(tmpItems);
            SelectedIndex = selectedIndex;
        }
    }
}
