using WpfMvvm.Models.Settings.Base;
using WpfMvvm.ViewModels.MainWindow.StatusBar;
using WpfMvvm.ViewModels.MainWindow.TopPanel;
using static WpfMvvm.Models.Settings.SettingsKnownParts;

namespace WpfMvvm.ViewModels.MainWindow
{
    internal static class ShutdownBehaviorsHelper
    {
        internal static void ShutdownBehaviors(MainWindowVM vm)
        {
            var settings = BaseSettings.Settings;
            SaveSettings(vm, settings);
        }

        private static void SaveSettings(MainWindowVM vm, BaseSettings settings)
        {
            SetSettings(settings, vm);
            settings.SaveToFileIfChanged();
        }

        private static void SetSettings(BaseSettings settings, MainWindowVM vm)
        {
            StatusBarVM statusBarVM = vm.StatusBarVM;
            TopPanelVM topPanelVM = GetTopPanelVM();

            var isDark = statusBarVM.ThemeSwitcherVM.IsChecked;
            var langIndex = statusBarVM.LangComboboxVM.SelectedIndex;
            var lang = statusBarVM.LangComboboxVM.Items[langIndex].Title;
            var isShowPrefix = topPanelVM.IsPublicKeyPrefixPresent;
            var isDeletedPresent = topPanelVM.IsDeletedPresent;
            
            settings.SetValue(SectionMain, MainIsDark, isDark.ToString());
            settings.SetValue(SectionMain, MainLang, lang);
            settings.SetValue(SectionMain, MainIsShowPubKeyPrefix, isShowPrefix.ToString());
            settings.SetValue(SectionMain, MainIsDeletedPresent, isDeletedPresent.ToString());
        }
    }
}
