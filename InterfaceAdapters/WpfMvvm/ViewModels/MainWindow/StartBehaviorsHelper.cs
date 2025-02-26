using System;
using System.Linq;
using System.Threading.Tasks;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.Models.Settings.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;
using WpfMvvm.ViewModels.MainWindow.StatusBar;
using WpfMvvm.ViewModels.MainWindow.TopPanel;
using static WpfMvvm.Models.Settings.SettingsHelper;
using static WpfMvvm.Models.Settings.SettingsKnownParts;

namespace WpfMvvm.ViewModels.MainWindow
{
    internal static class StartBehaviorsHelper
    {
        internal static async Task StartBehaviorsAsync(MainWindowVM vm)
        {
            var settings = BaseSettings.Settings;
            SetStatusBarPart(vm, settings);
            SetTopPanelPart(vm, settings);
            await vm.InitFieldsAsync();
        }

        internal static void StartBehaviors(MainWindowVM vm)
        {
            var langValue = GetMainSectionValue(BaseSettings.Settings, MainLang, MainLangDefault);
            var langVM = vm.StatusBarVM.LangComboboxVM;
            if (langVM.Items?.Count > 0)
                langVM.SelectedIndex = GetLangIndex(langVM, langValue);
        }

        #region Theme Part
        internal static void SetTheme()
        {
            var isDarkStringValue = GetMainSectionValue(BaseSettings.Settings, MainIsDark, MainIsDarkDefault);
            var isDarkValue = Convert.ToBoolean(isDarkStringValue);
            ExecuteThemeCommand(isDarkValue);
            var vm = GetMainWindowVM().StatusBarVM;
            vm.ThemeSwitcherVM.IsChecked = isDarkValue;
        }

        private static void ExecuteThemeCommand(bool isDark)
        {
            var cmd = new SwitchThemeCommand();
            cmd.Execute(isDark);
        }
        #endregion

        #region StatusBar Part
        private static void SetStatusBarPart(MainWindowVM vm, BaseSettings settings)
        {
            var statusBarVM = vm.StatusBarVM;
            SetLang(statusBarVM, settings);
        }
        #endregion

        #region Lang Part
        private static void SetLang(StatusBarVM vm, BaseSettings settings)
        {
            var langValue = GetMainSectionValue(settings, MainLang, MainLangDefault);
            ExecuteLangCommand(langValue);
        }

        private static void ExecuteLangCommand(string lang)
        {
            SwitchLangCommand langCommand = new();
            langCommand.Execute(lang);
        }

        private static int GetLangIndex(CtrlComboboxVM langVM, string lang) =>
            Enumerable.Range(0, langVM.Items.Count)
                .FirstOrDefault(i => EqualsWoCase(langVM.Items[i].Title, lang));

        private static bool EqualsWoCase(string value1, string value2) =>
            string.Equals(value1, value2, StringComparison.OrdinalIgnoreCase);
        #endregion

        #region TopPanel Part
        private static void SetTopPanelPart(MainWindowVM vm, BaseSettings settings)
        {
            var topPanelVM = vm.TopPanelVM;
            SetPublicKeyPrefix(topPanelVM, settings);
            SetIsDeletedPresent(topPanelVM, settings);
        }

        private static void SetPublicKeyPrefix(TopPanelVM vm, BaseSettings settings)
        {
            var value = GetMainSectionValue(settings, MainIsShowPubKeyPrefix, MainIsShowPubKeyPrefixDefault);
            vm.IsPublicKeyPrefixPresent = Convert.ToBoolean(value);
        }

        private static void SetIsDeletedPresent(TopPanelVM vm, BaseSettings settings)
        {
            var value = GetMainSectionValue(settings, MainIsDeletedPresent, MainIsDeletedPresentDefault);
            vm.IsDeletedPresent = Convert.ToBoolean(value);
        }
        #endregion
    }
}
