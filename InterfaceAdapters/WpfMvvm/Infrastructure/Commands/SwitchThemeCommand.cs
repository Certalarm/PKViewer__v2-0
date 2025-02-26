using WpfMvvm.Infrastructure.Commands.Base;
using static WpfMvvm.Infrastructure.Commands.SwitchThemeCommandHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public class SwitchThemeCommand : Command
    {
        public override bool CanExecute(object parameter) =>
            parameter is bool?;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) 
                return;
            bool isDark = (bool)parameter == true;
            SwitchTheme(isDark);
        }
    }
}
