using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ListViewGroupIsCheckedConverter : IMultiValueConverter
    {
        private static bool _isFirstRun = true;
        private static bool _isFirstClick = true;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not bool newState || values[1] is not KeyContainerType type)
                return null;

            if (_isFirstRun && !newState)
                return null;

            _isFirstRun = false;

            ChangeStateIfFirstClick(newState);

            if (IsSortOrFilterCall == true)
                return null;

            ChangeStateIfGroupCall(type, newState);
            SetCallers(isGroupCall: true);
            
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        private static void ChangeStateIfFirstClick(bool newState)
        {
            if (_isFirstClick && newState)
            {
                IsSortOrFilterCall = false;
                _isFirstClick = false;
            }
        }

        private static void ChangeStateIfGroupCall(KeyContainerType type, bool newState)
        {
            if (!IsGroupCall)
                return;
            ChangeStateFilteredItems(type, newState);
            SetCallers(isItemCall: true, isAllGroupCall: false);
            ChangeStateFirstHeader();
            SetCallers(isAllGroupCall: true);
        }

        private static void ChangeStateFilteredItems(KeyContainerType type, bool newState)
        {
            foreach (var item in FilterItemVMsByType(type))
            {
                SetCallers(isItemCall: false);
                item.IsChecked = newState;
            }
        }

        private static void ChangeStateFirstHeader()
        {
            var lv = GetMainListView();
            GetFirstHeaderAsToggleButton(lv).IsChecked = IsAllCheckboxesChecked(GetGroupCheckBoxes(lv));
        }

        private static bool IsAllCheckboxesChecked(IEnumerable<CheckBox> checkboxes) =>
            checkboxes.Count() == checkboxes.Count(x => x.IsChecked == true);
    }
}
