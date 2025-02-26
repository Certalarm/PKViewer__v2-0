using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using static WpfMvvm.Infrastructure.Converters.FlagKeyToControlHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class KeyToPairConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c) =>
            value is not CtrlComboboxItemVM itemVM
                ? CreateEmptyPair()
                : new(itemVM.Title, null);
    }
}
