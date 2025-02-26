using System.Collections.Generic;
using System.Windows;
using WpfMvvm.Infrastructure.Commands.Base;
using WpfMvvm.DataSource;
using WpfMvvm.ViewModels.MessageBox;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;
using static WpfMvvm.Infrastructure.Commands.ShowErrorsCommandHelper;


namespace WpfMvvm.Infrastructure.Commands
{
    public class ShowErrorsCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var langDict = FindLangDict();
            ShowMessage(BuildVM(langDict));
        }

        private static MessageBoxVM BuildVM(ResourceDictionary langDict)
        {
            var caption = GetCaption(langDict);
            var pairOk = GetPairOk();
            var pairError = GetPairError(langDict);
            var buttonVMs = GetButtonVMs(langDict);
            return new(caption, pairOk, pairError, buttonVMs);
        }

        private static string GetCaption(ResourceDictionary langDict) =>
            GetValue(langDict, __showErrorCaption);

        private static KeyValuePair<string, IList<string>> GetPairOk() =>
            new KeyValuePair<string, IList<string>>(null, null);

        private static KeyValuePair<string, IList<string>> GetPairError(ResourceDictionary langDict)
        {
            var titleError = GetTitleError(langDict);
            var localizedRootError = LocalizeRoot(DataModelsLoader.GetErrors(), langDict);
            IList<string> itemsError = [ DataModelsLoader.GetPlainTextErrorReport(localizedRootError) ];
            return new KeyValuePair<string, IList<string>>(titleError, itemsError);
        }

        private static string GetTitleError(ResourceDictionary langDict) =>
            GetValue(langDict, __showErrorTitleErrors);

        private static IList<ButtonVM> GetButtonVMs(ResourceDictionary langDict) =>
            [ new ButtonVM(GetValue(langDict, __buttonClose), new CloseWindowIntoChild(), GetMessageBox())];
    }
}
