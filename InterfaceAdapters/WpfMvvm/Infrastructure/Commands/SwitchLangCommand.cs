using WpfMvvm.Infrastructure.Commands.Base;
using static WpfMvvm.Infrastructure.Commands.SwitchCommandHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public class SwitchLangCommand : Command
    {
        private const string __uriPostfix = @";component/";
        internal const string __xamlName = "Strings.xaml";

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            if (parameter is string lang)
                ChangeLang(lang);
        }

        private static void ChangeLang(string lang)
        {
            RemoveResource(__xamlName);
            AddResource(__xamlName, __uriPostfix, lang);
            ChangeLangWoBindingElements();
        }

        private static void ChangeLangWoBindingElements()
        {
            var vm = GetTopPanelVM();
            vm.Refresh();
        }
    }
}
