using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ListViewAllGroupIsCheckedConverter: Converter
    {
        private static bool _isFirstRun = true;
        private static bool _isFirstClick = true;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not bool newState)
                return null;

            if (_isFirstRun && !newState)
                return null;

            _isFirstRun = false;

            ChangeStateIfFirstClick(newState);

            if (IsSortOrFilterCall == true)
                return null;

            ChangeStateIfAllGroupCall(newState);
            SetCallers(isAllGroupCall: true);
            return null;
        }

        private static void ChangeStateIfFirstClick(bool newState)
        {
            if (_isFirstClick && newState)
            {
                IsSortOrFilterCall = false;
                _isFirstClick = false;
            }
        }

        private static void ChangeStateIfAllGroupCall(bool newState)
        {
            if (!IsAllGroupCall)
                return;
            ChangeGroupCheckboxesState(newState);
            ChangeItemsState(newState);
            SetCallers(isGroupCall: true, isItemCall: true);
        }

        private static void ChangeGroupCheckboxesState(bool newState)
        {
            foreach (var cb in GetGroupCheckBoxes(GetMainListView()))
            {
                SetCallers(isGroupCall: false);
                cb.IsChecked = newState;
            }
        }

        private static void ChangeItemsState(bool newState)
        {
            foreach (var item in MainListViewItemVMs)
            {
                SetCallers(isItemCall: false);
                item.IsChecked = newState;
            }
        }
    }
}
