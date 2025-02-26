using System.Windows;
using WpfMvvm.Infrastructure.Commands.Base;

namespace WpfMvvm.Infrastructure.Commands
{
    public class CloseWindowIntoChild : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            if (parameter is not DependencyObject child)
                return;

            var window = RelativeParent<Window>(child);
            window?.Hide();
        }
    }
}
