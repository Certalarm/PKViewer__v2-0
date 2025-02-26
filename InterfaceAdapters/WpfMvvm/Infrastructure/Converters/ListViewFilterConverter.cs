using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ListViewFilterConverter : Converter
    {
        private static bool _isChecked;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            _isChecked = GetTopPanelVM().IsDeletedPresent;
            GetMainListViewVM().FilterItems(_isChecked);

            IsSortOrFilterCall = true;

            return value;
        }

        public override object ConvertBack(object value, Type t, object p, CultureInfo c) => !_isChecked;
    }
}
