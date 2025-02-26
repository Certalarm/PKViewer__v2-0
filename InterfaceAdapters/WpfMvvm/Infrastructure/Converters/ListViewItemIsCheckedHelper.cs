using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfMvvm.ViewModels.KeyContainerItem;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class ListViewItemIsCheckedHelper
    {
        internal static void ChangeStateIfSortOrFilterCall(ListViewItem lvItem)
        {
            SetCallers(isItemCall: false);
            ChangeStateIfLastItem(lvItem);
        }

        internal static void ChangeStateIfNotSortOrFilterCall(ListViewItem lvItem, bool newState)
        {
            ChangeStateIfItemCall(lvItem, newState);
            SetCallers(isItemCall: true);
        }

        private static void ChangeStateIfLastItem(ListViewItem lvItem)
        {
            var sortedItems = SortedItemVMs(GetMainListView());
            if (!IsLastItem(sortedItems, lvItem))
                return;
            SetCallers(isAllGroupCall: false);
            var lv = GetMainListView();
            var allCheckboxes = GetGroupCheckBoxes(lv).ToArray();
            ChangeGroupHeaderStates(sortedItems, allCheckboxes);
            GetFirstHeaderAsToggleButton(lv).IsChecked = IsAllCheckboxesChecked(allCheckboxes);
            SetCallers(isAllGroupCall: true, isGroupCall: true, isItemCall: true);
            IsSortOrFilterCall = false;
        }

        private static bool IsLastItem(IEnumerable<KeyContainerItemVM> items, ListViewItem lvItem)
        {
            var curItem = lvItem.DataContext as KeyContainerItemVM;
            var lastItem = items.Last();
            return curItem == lastItem;
        }

        private static KeyContainerType[] GetAllTypes(IEnumerable<KeyContainerItemVM> items) =>
            items
                .Select(x => x.Type)
                .Distinct()
                .ToArray();

        private static void ChangeGroupHeaderStates(IEnumerable<KeyContainerItemVM> items, CheckBox[] checkboxes)
        {
            var allTypes = GetAllTypes(items);
            for (int i = 0; i < allTypes.Length; i++)
            {
                SetCallers(isGroupCall: false);
                if (IsAllCheckedByType(items, allTypes[i]))
                    checkboxes[i].IsChecked = true;
            }
        }

        private static bool IsAllCheckedByType(IEnumerable<KeyContainerItemVM> items, KeyContainerType type)
        {
            var itemsOneType = items.Where(x => x.Type == type);
            var countAll = itemsOneType.Count();
            var countChecked = itemsOneType.Count(x => x.IsChecked == true);
            return countAll == countChecked;
        }

        private static bool IsAllCheckboxesChecked(IEnumerable<CheckBox> checkboxes) =>
            checkboxes.Count() == checkboxes.Count(x => x.IsChecked == true);

        private static void ChangeStateIfItemCall(ListViewItem lvItem, bool newState)
        {
            if (!IsItemCall)
                return;
            ChangeFirstColumnHeaderState(newState);
            ChangeCurrentGroupHeaderState(GetCurrentGroup(lvItem), newState);
        }

        private static void ChangeFirstColumnHeaderState(bool newState)
        {
            var allCount = MainListViewItemVMs.Count();
            var allCheckedCount = MainListViewItemVMs.Count(x => x.IsChecked);
            if (IsAllOrAllMinusOneChecked(newState, allCount, allCheckedCount))
            {
                SetCallers(isAllGroupCall: false, isGroupCall: false);
                GetFirstHeaderAsToggleButton(GetMainListView()).IsChecked = newState;
                SetCallers(isAllGroupCall: true, isGroupCall: true);
            }
        }

        private static void ChangeCurrentGroupHeaderState(GroupItem groupItem, bool newState)
        {
            var allItemsInGroup = ItemVMsInGroup(groupItem);
            var allCountInGroup = allItemsInGroup.Count();
            var checkedCountInGroup = allItemsInGroup.Count(x => x.IsChecked);
            if (IsAllOrAllMinusOneChecked(newState, allCountInGroup, checkedCountInGroup))
            {
                SetCallers(isAllGroupCall: false, isGroupCall: false);
                GetCurrentGroupHeader(groupItem).IsChecked = newState;
                SetCallers(isAllGroupCall: true, isGroupCall: true);
            }
        }

        private static bool IsAllOrAllMinusOneChecked(bool newState, int allCount, int allCheckedCount) =>
            IsNeedStateAndAllChecked(!newState, allCount - 1, allCheckedCount)
            || IsNeedStateAndAllChecked(newState, allCount, allCheckedCount);

        private static bool IsNeedStateAndAllChecked(bool newState, int allCount, int allCheckedCount) =>
            newState && allCount == allCheckedCount;
    }
}
