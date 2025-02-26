using System.Windows.Input;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.Models.Settings.Base;

namespace WpfMvvm.ViewModels.MainWindow
{
    internal static class SortMainLVBehaviorsHelper
    {
        private static ICommand _listViewSortCommand;

        internal static void SortMainLV(object clickedHeader)
        {
            _listViewSortCommand ??= new ListViewSortCommand();
            _listViewSortCommand.Execute(clickedHeader);
        }
    }
}
