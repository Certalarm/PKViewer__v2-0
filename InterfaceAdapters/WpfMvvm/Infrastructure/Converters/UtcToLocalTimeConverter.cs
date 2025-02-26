using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.Models.Settings;
using WpfMvvm.Models.Settings.Base;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class UtcToLocalTimeConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            var langDict = FindLangDict();
            return value is not DateTime utcTime
                ? langDict?[__noData] ?? string.Empty
                : FormatByCulture(utcTime.ToLocalTime(), langDict);
        }

        private static string FormatByCulture(DateTime dt, ResourceDictionary langDict) =>
            dt.ToString(langDict == null 
                ? __dateAndTimeFormatRu 
                : DetectDateTimeFormat(langDict)
            , CreateCultureByLang());

        private static string DetectDateTimeFormat(ResourceDictionary langDict) =>
            langDict.HasKey(__dateAndTimeFormat)
                ? langDict[__dateAndTimeFormat] as string
                : __dateAndTimeFormatRu;

        private static CultureInfo CreateCultureByLang() =>
            new (DetectLang());

        private static string DetectLang()
        {
            var mainVm = GetMainWindowVM();
            var vm = mainVm.StatusBarVM.LangComboboxVM;
            return vm.Items != null && vm.Items.Any() && vm.SelectedIndex > -1
                ? vm.Items[vm.SelectedIndex].Title
                : BaseSettings.Settings.GetValue(SettingsKnownParts.SectionMain,SettingsKnownParts.MainLang);
        }
    }
}
