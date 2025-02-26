using WpfMvvm.Infrastructure.Commands.Base;
using static WpfMvvm.Infrastructure.Commands.SwitchCommandHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public class LoadFlagsCommand : Command
    {
        internal const string __xamlName = "Flags.xaml";
        private const string __uriPostfix = @";component/";
        private const string __dllName = "CountryFlags";

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter = default)
        {
                LoadFlags(__dllName);
        }

        private static void LoadFlags(string dllName)
        {
            RemoveResource(__xamlName);
            AddResource(__xamlName, __uriPostfix, dllName);
        }
    }
}
