using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using WpfMvvm.Infrastructure.Commands.Base;
using WpfMvvm.ViewModels.MessageBox;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public class OpenWwwCommand: Command
    {
        public override bool CanExecute(object parameter) => parameter is string;

        public override void Execute(object parameter)
        {
            if (parameter is not string homePageUrl || string.IsNullOrWhiteSpace(homePageUrl))
                return;

            var error = StartProcess(homePageUrl);
            if (string.IsNullOrWhiteSpace(error))
                return;

            ShowErrorMessage(error);
        }

        private static string StartProcess(string url)
        {
            string error = string.Empty;
            try
            {
                Process.Start(url);
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }
            return error;
        }

        private static void ShowErrorMessage(string error)
        {
            var langDict = FindLangDict();
            var errorVM = BuildErrorVM(langDict, error);
            ShowMessage(errorVM);
        }

        private static MessageBoxVM BuildErrorVM(ResourceDictionary langDict, string error)
        {
            var errorMessage = BuildErrorMessage(error, langDict);
            var caption = GetValue(langDict, __errorCaption);
            var cancel = GetValue(langDict, __buttonCancel);
            var cancelVM = new ButtonVM(cancel, new CloseWindowIntoChild(), GetMessageBox());
            var pairOk = new KeyValuePair<string, IList<string>>(errorMessage, null);
            var pairError = new KeyValuePair<string, IList<string>>();
            return new MessageBoxVM(caption, pairOk, pairError, [cancelVM]);
        }

        private static string BuildErrorMessage(string error, ResourceDictionary langDict) =>
            $"{GetValue(langDict, __btnWebError)}{Environment.NewLine}   {error}";
    }
}
