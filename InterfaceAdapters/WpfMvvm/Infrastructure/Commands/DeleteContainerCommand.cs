using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfMvvm.Infrastructure.Commands.Base;
using WpfMvvm.DataSource;
using static WpfMvvm.Infrastructure.Commands.DeleteContainerCommandMBoxVmBuilder;

namespace WpfMvvm.Infrastructure.Commands
{
    public class DeleteContainerCommand : CommandAsync
    {
        protected override Task ExecuteAsync(object p)
        {
            var taskIfOne = DeleteIfOnePathAndShowMessage(p);
            var taskIfMany = DeleteIfManyPathsAndShowMessage(p);
            return Task.WhenAll(taskIfOne, taskIfMany);
        }

        private static async Task DeleteIfOnePathAndShowMessage(object parameter)
        {
            if (parameter is string path)
                await DeleteAndShowMessage(path);

            if (parameter is IEnumerable<string> paths && paths != null && paths.Count() == 1)
                await DeleteAndShowMessage(paths.First());
        }

        private static async Task DeleteIfManyPathsAndShowMessage(object parameter)
        {
            if (parameter is IEnumerable<string> paths && paths != null && paths.Count() > 1)
                await DeleteAllAndShowMessage(paths);
        }

        private static async Task DeleteAndShowMessage(string containerPath)
        {
            var errorOrContainerFullPath = DataModelsLoader.DeleteContainer(containerPath);
            var messageBoxVM = CreateMessageBoxVmIfOne(containerPath, errorOrContainerFullPath);
            await UpdateItemsAfterDelete();
            ShowMessage(messageBoxVM);
        }

        private static async Task DeleteAllAndShowMessage(IEnumerable<string> containerPaths)
        {
            var errorsOrContainerFullPaths = DataModelsLoader.DeleteContainers(containerPaths);
            var messageBoxVM = CreateMessageBoxVmIfMany(containerPaths.ToList(), errorsOrContainerFullPaths);
            await UpdateItemsAfterDelete();
            ShowMessage(messageBoxVM);
        }

        private static async Task UpdateItemsAfterDelete()
        {
            if (IsNotNeedUpdate())
                return;

            var topVM = GetTopPanelVM();

            var readerIndex = topVM.ReaderComboBoxVM.SelectedIndex;
            var reader = topVM.ReaderComboBoxVM.Items[readerIndex].Title;

            var keyMediaIndex = topVM.KeyMediaComboBoxVM.SelectedIndex;
            var keyMedia = topVM.KeyMediaComboBoxVM.Items[keyMediaIndex].Title;

            var isDeletedPresent = topVM.IsDeletedPresent;
            await GetMainListViewVM().UpdateItemsAsync(reader, keyMedia, isDeletedPresent, true);
        }

        private static bool IsNotNeedUpdate() =>
            GetItemsCountBeforeDelete() == GetItemsCountAfterDelete();

        private static int GetItemsCountBeforeDelete() =>
            GetMainListViewVM().Items.Count;

        private static int GetItemsCountAfterDelete() =>
            GetCollectionView(GetMainListView()).SourceCollection
                .OfType<ListViewItem>()
                .Count();
    }
}
