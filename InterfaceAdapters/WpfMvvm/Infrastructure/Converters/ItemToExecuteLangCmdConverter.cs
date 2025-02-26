using System.Linq;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public sealed class ItemToExecuteLangCmdConverter : SelectionChangedConverter
    {
        protected override void ExecuteCommand(int index)
        {
            _command ??= new SwitchLangCommand();
            if(_ctrlComboboxVM != null && _ctrlComboboxVM.Items.Any())
                _command.Execute(_ctrlComboboxVM.Items[index].Title);
        }
    }
}
