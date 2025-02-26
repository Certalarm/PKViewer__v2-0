using System.Windows;
using WpfMvvm.Infrastructure.Commands.Base;

namespace WpfMvvm.Infrastructure.Commands
{
    public class CloseWindowCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
