using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using static WpfMvvm.Infrastructure.Converters.FlagKeyToControlHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class FlagKeyToControlConverter : Converter
    {
        private static LoadFlagsCommand _cmd;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not CtrlComboboxItemVM itemVM)
                return CreateEmptyPair();

            LoadFlagsDict();
            return FindFlag(itemVM.Title);
        }

        private static void LoadFlagsDict()
        {
            if (_cmd != null)
                return;
            _cmd = new();
            _cmd.Execute();
        }
    }
}
