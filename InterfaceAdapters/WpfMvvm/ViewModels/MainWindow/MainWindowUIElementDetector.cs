using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WpfMvvm.ViewModels.MainWindow
{
    public static class MainWindowUIElementDetector
    {
        private const string __messageBox = "MessageBox";

        internal static Window GetMainWindow() => Application.Current.MainWindow;

        internal static ListView GetMainListView() => RelativeChild<ListView>(GetMainWindow());

        internal static GroupItem GetCurrentGroup(ListViewItem lvItem) =>
            RelativeParent<GroupItem>(lvItem);

        internal static CheckBox GetCurrentGroupHeader(GroupItem groupItem) =>
            RelativeChild<Expander>(groupItem).Header as CheckBox;

        internal static ICollectionView GetCollectionView(ListView lv) => 
            CollectionViewSource.GetDefaultView(lv?.ItemsSource);

        internal static CollectionViewGroup GetCollectionViewGroup(GroupItem groupItem) =>
            groupItem.DataContext as CollectionViewGroup;

        internal static IEnumerable<GridViewColumnHeader> GetListViewHeaders(ListView lv) =>
            lv.FindVisualChildren<GridViewColumnHeader>();

        internal static GridViewColumnHeader GetListViewHeader(ListView lv, int index)
        {
            var headers = GetListViewHeaders(lv);
            int headersCount = headers.Count();
            return headers.Skip(headersCount - 2 + index).First();
        }

        internal static ToggleButton GetFirstHeaderAsToggleButton(ListView lv) =>
            RelativeChild<ToggleButton>(GetListViewHeader(lv, 0));

        internal static IEnumerable<CheckBox> GetGroupCheckBoxes(ListView lv) =>
            GetListViewGroupHeaders(lv)
                .Select(x => x as CheckBox);

        internal static IEnumerable<object> GetListViewGroupHeaders(ListView lv) =>
            lv.FindVisualChildren<Expander>()
                .Select(x => x.Header);

        internal static Window GetMessageBox() =>
            Application.Current.FindResource(__messageBox) as Window;
    }
}
