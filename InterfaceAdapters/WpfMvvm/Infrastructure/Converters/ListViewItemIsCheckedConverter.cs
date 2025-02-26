using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;
using static WpfMvvm.Infrastructure.Converters.ListViewItemIsCheckedHelper;


namespace WpfMvvm.Infrastructure.Converters
{
    public class ListViewItemIsCheckedConverter : IMultiValueConverter
    {
        private static bool _isFirstRun = true;
        private static bool _isFirstClick = true;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not bool newState || values[1] is not ListViewItem lvItem)
                return null;

            if (_isFirstRun && !newState)
                return null;

            _isFirstRun = false;

            GetMainListViewVM().UpdateCheckedCount();
            ChangeStateIfFirstClick(newState);

            if (IsSortOrFilterCall)
                ChangeStateIfSortOrFilterCall(lvItem);
            else
                ChangeStateIfNotSortOrFilterCall(lvItem, newState);

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
    }
}
