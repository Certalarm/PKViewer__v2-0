using System.Threading.Tasks;
using WpfMvvm.Infrastructure.Commands.Base;

namespace WpfMvvm.Infrastructure.Commands
{
    public class UpdateReadersCommand : CommandAsync
    {
        protected override bool CanExecute(object parameter) => true;

        protected override Task ExecuteAsync(object p)
        {
            var lvVm = GetMainListViewVM();
            lvVm.ShowPreloader();
            var tpVm = GetTopPanelVM();
            return tpVm.InitFieldsAsync();
        }
    }
}
