using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using WpfMvvm.ViewModels.MessageBox;
using static WpfMvvm.Infrastructure.Commands.ErrorLocalizeHelper;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;
using static WpfMvvm.Infrastructure.Commands.SaveCertCommandHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class SaveCertCommandMBoxVmBuilder
    {
        private static ResourceDictionary _langDict;

        internal static MessageBoxVM CreateMessageBoxVmIfOne(string containerPath, string certPath)
        {
            _langDict = FindLangDict();
            var caption = CreateCaption();
            var pairOkOrError = CreatePairOkOrErrorIfOne(containerPath, certPath);
            var buttonVMs = CreateButtonVMs(certPath, IsError(certPath));
            var question = CreateQuestion(IsError(certPath));
            return new MessageBoxVM(caption, pairOkOrError, NullPair(), buttonVMs, question);
        }

        internal static MessageBoxVM CreateMessageBoxVmIfMany(IList<string> containerPaths, IList<string> errorsOrCertPaths)
        {
            _langDict = FindLangDict();
            var caption = CreateCaption();
            (IList<string> errorContainerPaths, IList<string> okContainerPaths) =
                SplitByErrorsAndPaths(containerPaths, errorsOrCertPaths);
            var pairOk = CreatePairOkIfMany(errorsOrCertPaths, okContainerPaths);
            var pairError = CreatePairErrorIfMany(errorsOrCertPaths, errorContainerPaths);
            var buttonVMs = CreateButtonVMs(GetCertPath(errorsOrCertPaths), !HasPaths(errorsOrCertPaths));
            var question = CreateQuestion(!HasPaths(errorsOrCertPaths));
            return new MessageBoxVM(caption, pairOk, pairError, buttonVMs, question);
        }

        private static string CreateCaption() =>
            Localize(__savingCertCaption);

        private static KeyValuePair<string, IList<string>> CreatePairOkOrErrorIfOne(
            string containerPath, string certPath) 
            => new (CreateTitleIfOne(containerPath, certPath), null);

        private static KeyValuePair<string, IList<string>> CreatePairOkIfMany(
            IList<string> errorsOrPaths, IList<string> okContainerPaths)
        {
            if (!HasPaths(errorsOrPaths))
                return NullPair();
            var keyOk = CreateTitleOKIfMany(GetPaths(errorsOrPaths).First());
            var valueOk = CreateItemsOKIfMany(okContainerPaths, GetPaths(errorsOrPaths));
            return new(keyOk, valueOk.ToList());
        }

        private static KeyValuePair<string, IList<string>> CreatePairErrorIfMany(
            IList<string> errorsOrPaths, IList<string> errorContainerPaths)
        {
            if (!HasErrors(errorsOrPaths))
                return NullPair();
            var keyError = CreateTitleErrorIfMany();
            var valueError = CreateItemsErrorIfMany(errorContainerPaths, GetErrors(errorsOrPaths));
            return new(keyError, valueError.ToList());
        }

        private static string CreateTitleIfOne(string containerPath, string errorOrCertFullPath) =>
            IsError(errorOrCertFullPath)
                ? BuildErrorTitleIfOne(containerPath, errorOrCertFullPath)
                : BuildOkTitleIfOne(Localize(__savingCertBody), containerPath, errorOrCertFullPath);

        private static string CreateTitleOKIfMany(string certFullPath) =>
            string.Format(Localize(__savingCertsTitleOk), Path.GetDirectoryName(certFullPath));

        private static IEnumerable<string> CreateItemsOKIfMany(IList<string> containerPaths, IList<string> certFullPaths)
        {
            for (int i = 0; i < containerPaths.Count; i++)
                yield return (string.Format(Localize(__savingCertsItemOk), containerPaths[i])
                    + $"{Environment.NewLine} {Path.GetFileName(certFullPaths[i])}");
        }

        private static string CreateTitleErrorIfMany() =>
            Localize(__savingCertsTitleError);

        private static IEnumerable<string> CreateItemsErrorIfMany(IList<string> containerPaths, IList<string> errors)
        {
            for (int i = 0; i < containerPaths.Count; i++)
                yield return BuildErrorItemIfMany(containerPaths[i], errors[i]);
        }

        private static IList<ButtonVM> CreateButtonVMs(string certPath, bool condition)
        {
            var cancel = Localize(__buttonCancel);
            var cancelVM = CreateCancelVM(GetMessageBox(), cancel);
            if (condition)
                return [cancelVM];
            var yes = Localize(__buttonYes);
            return [CreateYesVM(certPath, yes), cancelVM];
        }

        private static string CreateQuestion(bool condition) =>
            condition
                ? null
                : Localize(__savingCertQuestion);

        private static ButtonVM CreateCancelVM(DependencyObject cmdParam, string caption) =>
            new(caption, new CloseWindowIntoChild(), cmdParam);

        private static ButtonVM CreateYesVM(string cmdParam, string caption) =>
            new(caption, new OpenFolderCommand(), cmdParam);

        private static string BuildErrorTitleIfOne(string containerPath, string error)
        {
            var (errorKey, errorDesc) = GetLocalizeError(error);
            return $"{errorKey} '{containerPath}':{Environment.NewLine}- {errorDesc}";
        }

        private static string BuildOkTitleIfOne(string titleBody, string containerPath, string certFullPath) =>
            string.Format(titleBody, containerPath, Path.GetFileName(certFullPath))
                + $"{Environment.NewLine} {Path.GetDirectoryName(certFullPath)}";

        private static string BuildErrorItemIfMany(string containerPath, string error)
        {
            var (errorKey, errorDesc) = GetLocalizeError(error);
            return string.Format(Localize(__savingCertsItemError), containerPath)
                + $"{Environment.NewLine}- {errorKey}: {errorDesc}";
        }

        private static string GetCertPath(IEnumerable<string> errorsOrCertPaths) =>
            errorsOrCertPaths
                .FirstOrDefault(x => !IsError(x));

        private static string Localize(string value) =>
            GetValue(_langDict, value);
    }
}
