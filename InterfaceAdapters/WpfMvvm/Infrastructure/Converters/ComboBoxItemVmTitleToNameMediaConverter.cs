using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.DataSource;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;
using static WpfMvvm.Infrastructure.Converters.KeyMediaSelectionChangedHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class ComboBoxItemVmTitleToNameMediaConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not CtrlComboboxItemVM itemVm)
                return EmptyPair();

            var displayName = IsAllKeysMedia(itemVm.Title)
                ? GetValue(FindLangDict(), itemVm.Title)
                : GetDisplayNameIfRootPath(itemVm.Title);
            return new KeyValuePair<string, ContentControl>(displayName, null);
        }

        private static KeyValuePair<string, ContentControl> EmptyPair() =>
            new(string.Empty, null);

        private static bool IsAllKeysMedia(string title) =>
            string.Equals(title, __allKeysMedia, StringComparison.OrdinalIgnoreCase);

        private static string GetDisplayNameIfRootPath(string rootPath)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
                return string.Empty;
            var keyMedia = DataModelsLoader.GetKeyMediaInfo(rootPath);
            return GetKeyMediaDisplayName(keyMedia);
        }
    }
}
