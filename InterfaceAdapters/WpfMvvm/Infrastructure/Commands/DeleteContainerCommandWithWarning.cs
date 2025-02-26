using WpfMvvm.Infrastructure.Commands.Base;
using static WpfMvvm.Infrastructure.Commands.DeleteContainerCommandMBoxVmBuilder;

namespace WpfMvvm.Infrastructure.Commands
{
    public class DeleteContainerCommandWithWarning : Command
    {
        public override bool CanExecute(object parameter) => GetMainListViewVM().Items.Count > 0;

        public override void Execute(object parameter)
        {
            ShowMessage(CreateMessageBoxVmIfWarning(parameter));
        }
    }
}
