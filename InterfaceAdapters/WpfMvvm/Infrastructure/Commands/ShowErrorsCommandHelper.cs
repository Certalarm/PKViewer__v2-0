using PKInfo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using static WpfMvvm.Infrastructure.Commands.ErrorLocalizeHelper;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class ShowErrorsCommandHelper
    {
        private static ResourceDictionary _langDict;

        internal static RootError LocalizeRoot(RootError rootError, ResourceDictionary langDict)
        {
            _langDict = langDict;
            return LocalizeRootError(rootError);
        }

        private static RootError LocalizeRootError(RootError rootError)
        {
            var localizeReaderErrors = rootError.ReaderErrors
                .Select(LocalizeReaderError)
                .ToList();
            LocalizeSnapshotTime(rootError.Content);
            return RootError.Create(LocalizeContent(rootError.Content), localizeReaderErrors);
        }

        private static void LocalizeSnapshotTime(Dictionary<string, string> content)
        {
            if (!content.ContainsKey(__snapshotTime))
                return;
            if(!DateTime.TryParse(content[__snapshotTime], out var parsedTime))
                return;
            content[__snapshotTime] = parsedTime.ToString(Localize(__dateAndTimeFormat), CultureInfo.InvariantCulture);
        }

        private static ReaderError LocalizeReaderError(ReaderError readerError)
        {
            var localizeMediaErrors = readerError.KeyMediaErrors
                .Select(LocalizeKeyMediaError)
                .ToList();
            return ReaderError.Create(LocalizeContent(readerError.Content), localizeMediaErrors);
        }

        private static KeyMediaError LocalizeKeyMediaError(KeyMediaError mediaError)
        {
            var localizeContainerTypeErrors = mediaError.KeyContainerTypeErrors
                .Select(LocalizeContainerTypeError)
                .ToList();
            return KeyMediaError.Create(LocalizeContent(mediaError.Content), localizeContainerTypeErrors);
        }

        private static KeyContainerTypeError LocalizeContainerTypeError(KeyContainerTypeError typeError)
        {
            var localizeContainerErrors = typeError.KeyContainerErrors
                .Select(LocalizeContainerError)
                .ToList();
            return KeyContainerTypeError.Create(LocalizeContent(typeError.Content), localizeContainerErrors);
        }

        private static KeyContainerError LocalizeContainerError(KeyContainerError error) =>
            KeyContainerError.Create(LocalizeContent(error.Content));    

        private static Dictionary<string, string> LocalizeContent(Dictionary<string, string> content)
        {
            Dictionary<string, string> result = [];
            foreach(var item in content)
            {
                var localizeItem = LocalizeContentItem(item);
                result[localizeItem.Key] = localizeItem.Value;
            }
            return result;
        }

        private static KeyValuePair<string, string> LocalizeContentItem(KeyValuePair<string, string> item) =>
            new(Localize(item.Key), LocalizeMayBeError(item.Value));

        private static string LocalizeMayBeError(string value) =>
            IsError(value)
                ? LocalizeError(value)
                : Localize(value);

        private static string LocalizeError(string error)
        {
            var (errorKey, errorDesc) = GetLocalizeError(error);
            return $"{errorKey}: {errorDesc}";
        }

        private static string Localize(string value) =>
            GetValue(_langDict, value);
    }
}
