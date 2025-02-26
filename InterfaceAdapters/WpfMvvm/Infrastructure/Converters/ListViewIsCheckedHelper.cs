using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfMvvm.ViewModels.KeyContainerItem;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class ListViewIsCheckedHelper
    {
        internal static bool IsItemCall { get; set; } = true;
        internal static bool IsGroupCall { get; set; } = true;
        internal static bool IsAllGroupCall { get; set; } = true;

        internal static bool IsSortOrFilterCall { get; set; } = false;

        internal static IEnumerable<KeyContainerItemVM> MainListViewItemVMs =>
            GetMainListViewVM().Items;

        internal static void SetCallers(bool? isAllGroupCall = default, bool? isGroupCall = default, bool? isItemCall = default)
        {
            if (isAllGroupCall != default)
                IsAllGroupCall = isAllGroupCall == true;
            if (isGroupCall != default)
                IsGroupCall = isGroupCall == true;
            if (isItemCall != default)
                IsItemCall = isItemCall == true;
        }

        internal static IEnumerable<KeyContainerItemVM> SortedItemVMs(ListView lv) =>
            lv.Items.OfType<KeyContainerItemVM>();

        internal static IEnumerable<KeyContainerItemVM> FilterItemVMsByType(KeyContainerType type) =>
            MainListViewItemVMs
                .Where(x => x.Type == type);

        internal static IEnumerable<KeyContainerItemVM> ItemVMsInGroup(GroupItem groupItem) =>
            GetCollectionViewGroup(groupItem).Items
                .OfType<KeyContainerItemVM>();
    }
}
