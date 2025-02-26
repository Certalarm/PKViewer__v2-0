using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfMvvm.ViewModels.MessageBox;
using static PKInfo.Utility.Txt;
using static WpfMvvm.Infrastructure.Commands.DeleteContainerCommandHelper;
using static WpfMvvm.Infrastructure.Commands.ErrorLocalizeHelper;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class DeleteContainerCommandMBoxVmBuilder
    {
        private static ResourceDictionary _langDict;

        internal static MessageBoxVM CreateMessageBoxVmIfWarning(object parameter)
        {
            _langDict = FindLangDict();
            var caption = Localize(__warningCaption);
            ButtonVM yesVm = new (Localize(__buttonYes), new DeleteContainerCommand(), parameter);
            ButtonVM cancelVm = CreateCloseVM(GetMessageBox(), Localize(__buttonCancel));
            var question = Localize(__deleteWarningQuestion);
            return new(caption, NullPair(), NullPair(), [yesVm, cancelVm], question);
        }

        internal static MessageBoxVM CreateMessageBoxVmIfOne(string containerPath, string errorOrEmpty)
        {
            _langDict = FindLangDict();
            var caption = CreateCaption();
            var pairOkOrError = CreatePairOkOrErrorIfOne(containerPath, errorOrEmpty);
            var buttonVMs = CreateButtonVMs();
            return new MessageBoxVM(caption, pairOkOrError, NullPair(), buttonVMs);
        }

        internal static MessageBoxVM CreateMessageBoxVmIfMany(IList<string> containerPaths, IList<string> errorsOrEmpties)
        {
            _langDict = FindLangDict();
            var caption = CreateCaption();
            (IList<string> errorContainerPaths, IList<string> Empties) =
                SplitByErrorsAndEmpties(containerPaths, errorsOrEmpties);
            var pairOk = CreatePairOkIfMany(Empties);
            var pairError = CreatePairErrorIfMany(errorsOrEmpties, errorContainerPaths);
            var buttonVMs = CreateButtonVMs();
            return new MessageBoxVM(caption, pairOk, pairError, buttonVMs);
        }

        private static string CreateCaption() =>
            Localize(__deletingContainerCaption);

        private static KeyValuePair<string, IList<string>> CreatePairOkOrErrorIfOne(
            string containerPath, string errorOrEmpty)
            => new(CreateTitleIfOne(containerPath, errorOrEmpty), null);

        private static string CreateTitleIfOne(string containerPath, string errorOrEmpty) =>
            !string.IsNullOrWhiteSpace(errorOrEmpty)
                ? BuildErrorTitleIfOne(containerPath, errorOrEmpty)
                : BuildOkTitleIfOne(BuildTitleBodyIfOne(containerPath), containerPath);

        private static string BuildErrorTitleIfOne(string containerPath, string error)
        {
            var (errorKey, errorDesc) = GetLocalizeError(error);
            return string.Format(Localize(__deletingContainerBodyError), containerPath) +
                $"{Environment.NewLine}{errorKey}: {errorDesc}";
        }

        private static string BuildTitleBodyIfOne(string containerPath) =>
            string.Format(Localize(SelectTitleBody(containerPath)), containerPath);

        private static string SelectTitleBody(string containerPath) =>
            containerPath.Contains(DELETED_KEY_DIR_NAME)
                ? __deletingContainerBodyPermanently
                : __deletingContainerBodyToTrash;

        private static string BuildOkTitleIfOne(string titleBody, string containerPath) =>
            string.Format(titleBody, containerPath);

        private static IList<ButtonVM> CreateButtonVMs()
        {
            var close = Localize(__buttonClose);
            var closeVM = CreateCloseVM(GetMessageBox(), close);
            return [ closeVM ];
        }

        private static KeyValuePair<string, IList<string>> CreatePairOkIfMany(IList<string> Empties)
        {
            if (!Empties.Any())
                return NullPair();
            var keyOk = CreateTitleOKIfMany();
            var valueOk = CreateItemsOKIfMany(Empties);
            return new(keyOk, valueOk.ToList());
        }

        private static string CreateTitleOKIfMany() =>
            Localize(__deletingContainersTitleOk);

        private static IEnumerable<string> CreateItemsOKIfMany(IList<string> Empties)
        {
            for (int i = 0; i < Empties.Count; i++)
                yield return BuildTitleBodyIfOne(Empties[i]);
        }

        private static KeyValuePair<string, IList<string>> CreatePairErrorIfMany(
            IList<string> errorsOrEmpties, IList<string> errorContainerPaths)
        {
            if (!errorContainerPaths.Any())
                return NullPair();
            var keyError = CreateTitleErrorIfMany();
            var valueError = CreateItemsErrorIfMany(errorContainerPaths, GetErrors(errorsOrEmpties));
            return new(keyError, valueError.ToList());
        }

        private static string CreateTitleErrorIfMany() =>
            Localize(__deletingContainersTitleError);

        private static IEnumerable<string> CreateItemsErrorIfMany(IList<string> containerPaths, IList<string> errors)
        {
            for (int i = 0; i < containerPaths.Count; i++)
                yield return BuildErrorItemIfMany(containerPaths[i], errors[i]);
        }

        private static string BuildErrorItemIfMany(string containerPath, string error)
        {
            var (errorKey, errorDesc) = GetLocalizeError(error);
            return string.Format(Localize(__deletingContainersItemError), containerPath)
                + $"{Environment.NewLine}- {errorKey}: {errorDesc}";
        }

        private static ButtonVM CreateCloseVM(DependencyObject cmdParam, string caption) =>
            new(caption, new CloseWindowIntoChild(), cmdParam);

        private static string Localize(string value) =>
            GetValue(_langDict, value);

        private static IList<string> GetErrors(IList<string> errorsOrEmpties) =>
            errorsOrEmpties
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
    }
}
