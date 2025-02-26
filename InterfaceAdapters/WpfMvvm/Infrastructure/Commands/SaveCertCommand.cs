using System.Collections.Generic;
using System.Linq;
using WpfMvvm.Infrastructure.Commands.Base;
using WpfMvvm.DataSource;
using static WpfMvvm.Infrastructure.Commands.SaveCertCommandMBoxVmBuilder;

namespace WpfMvvm.Infrastructure.Commands
{
    public class SaveCertCommand : Command
    {
        public override bool CanExecute(object parameter) => GetMainListViewVM().Items.Count > 0;

        public override void Execute(object parameter)
        {
            SaveIfOnePathAndShowMessage(parameter);
            SaveIfManyPathsAndShowMessage(parameter);
        }

        private static void SaveIfOnePathAndShowMessage(object parameter)
        {
            if (parameter is string path)
                SaveCertAndShowMessage(path);

            if (parameter is IEnumerable<string> paths && paths != null && paths.Count() == 1)
                SaveCertAndShowMessage(paths.First());
        }

        private static void SaveIfManyPathsAndShowMessage(object parameter)
        {
            if (parameter is IEnumerable<string> paths && paths != null && paths.Count() > 1)
                SaveAllCertsAndShowMessage(paths);
        }

        private static void SaveCertAndShowMessage(string containerPath)
        {
            var errorOrCertFullPath = DataModelsLoader.SaveCert(containerPath);
            var messageBoxVM = CreateMessageBoxVmIfOne(containerPath, errorOrCertFullPath);
            ShowMessage(messageBoxVM);
        }

        private static void SaveAllCertsAndShowMessage(IEnumerable<string> containerPaths)
        {
            var errorsOrCertFullPaths = DataModelsLoader.SaveCerts(containerPaths);
            var messageBoxVM = CreateMessageBoxVmIfMany(containerPaths.ToList(), errorsOrCertFullPaths);
            ShowMessage(messageBoxVM);
        }
    }
}
