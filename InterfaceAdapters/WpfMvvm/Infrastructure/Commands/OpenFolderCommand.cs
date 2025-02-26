using System.Diagnostics;
using System.IO;
using WpfMvvm.Infrastructure.Commands.Base;

namespace WpfMvvm.Infrastructure.Commands
{
    public class OpenFolderCommand : Command
    {
        private const string __explorerName = "explorer.exe";

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            if (parameter is not string fullFilename || fullFilename.Length < 1)
                return;

            CloseMessageBox();
            OpenFolder(fullFilename);
        }

        private static void CloseMessageBox()
        {
            var closeCmd = new CloseWindowIntoChild();
            closeCmd.Execute(GetMessageBox());
        }

        private static void OpenFolder(string fullFilename)
        {
            var fullDirName = Path.GetDirectoryName(fullFilename);
            StartProcess(__explorerName, fullDirName);
        }

        private static void StartProcess(string processName, string args)
        {
            using Process proc = new();
            proc.StartInfo.FileName = processName;
            proc.StartInfo.Arguments = args;
            proc.Start();
        }
    }
}
