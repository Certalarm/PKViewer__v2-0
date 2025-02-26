using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ComboBoxItemVmTitleToPairConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if(value is not CtrlComboboxItemVM itemVm)
                return EmptyPair();
            var langDict = FindLangDict();
            return new KeyValuePair<string, ContentControl>(GetValue(langDict, itemVm.Title), null);
        }

        private static KeyValuePair<string, ContentControl> EmptyPair() =>
            new(string.Empty, null);
    }
}
